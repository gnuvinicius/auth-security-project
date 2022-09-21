using Security.Dtos;
using Security.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.Services;

public class AuthService : IAuthService
{
    private readonly SecurityContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(SecurityContext context, ILogger<AuthService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<UserDto> CreateUser(UserDto user)
    {

        try
        {
            Validate(user);

            var salt = _configuration["salt"];

            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password, Int32.Parse(salt));

            var persist = new User() {
                Username = user.Username,
                Password = hashed,
                Role = "user",
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogWarning("User to persist {0}", persist.Username);

            await _context.Users.AddAsync(persist);
            await _context.SaveChangesAsync();

            var dto = new UserDto()
            {
                Id = persist.Id,
                Username = persist.Username,
                Roles = new List<string>{ persist.Role }
            };

            return dto;
        }
        catch (System.Exception ex)
        {
            _logger.LogError("Error {0}", ex.Message);
            throw;
        }
    }

    public AuthResponseDto GenerateToken(AuthRequestDto auth)
    {
        
        User user = _context.Users.Where(u => u.Username == auth.Username).First();

        var verify = BCrypt.Net.BCrypt.Verify(auth.Password, user.Password);

        if (verify)
        {
            var secret = _configuration["secret"];

            JwtSecurityTokenHandler tokenHandler = new();
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
        else
        {
            throw new Exception(message: "wrong password");
        }
    }

    #region PRIVATE METHODS

    private void Validate(UserDto user) {
        
        var alreadyExists = _context.Users.Where(u => u.Username == user.Username).Any();

        if (alreadyExists) {
            throw new Exception("user already exists");
        }
    }

    #endregion

}
