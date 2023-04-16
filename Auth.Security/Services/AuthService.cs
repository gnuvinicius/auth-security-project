using Security.Dtos;
using Security.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.Services;

public class AuthService : IAuthService
{
    private const string Message = "User to persist {s}";
    private const string Message1 = "Error {s}";
    private readonly SecurityContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(SecurityContext context, ILogger<AuthService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<UserDto> CreateUser(AuthRequestDto user)
    {
        try
        {
            Validate(user);

            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password, Int32.Parse(_configuration["salt"]));
            User persist = await PersistNewUser(user, hashed);

            return new UserDto(persist);
        }
        catch (Exception ex)
        {
            _logger.LogError(Message1, ex.Message);
            throw;
        }
    }

    public AuthResponseDto Authenticate(AuthRequestDto auth)
    {
        User user = _context.Users.Where(u => u.Username == auth.Username).First();

        var verify = BCrypt.Net.BCrypt.Verify(auth.Password, user.Password);

        if (verify)
        {
            return GenerateToken(user);
        }
        else
        {
            throw new Exception(message: "wrong password");
        }
    }

    #region PRIVATE METHODS

    private AuthResponseDto GenerateToken(User user)
    {
        var secret = _configuration["secret"];

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(user.Username) ? user.Username : ""),
                new Claim("Store", !string.IsNullOrEmpty(user.Role) ? user.Role : "")
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        string token = tokenHandler.WriteToken(securityToken);

        AuthResponseDto response = new()
        {
            Type = "Bearer",
            Token = token,
        };

        return response;
    }

    private void Validate(AuthRequestDto user) {
        
        var alreadyExists = _context.Users.Where(u => u.Username == user.Username).Any();

        if (alreadyExists) {
            throw new Exception("user already exists");
        }
    }

    private async Task<User> PersistNewUser(AuthRequestDto dto, string hashed)
    {
        _logger.LogInformation(Message, dto.Username);

        var persist = new User(dto.Username, hashed, "role");

        await _context.Users.AddAsync(persist);
        await _context.SaveChangesAsync();
        return persist;
    }

    #endregion

}
