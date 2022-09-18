using Security.Dtos;
using Security.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    private readonly SecurityContext _context;

    public AuthService(IConfiguration configuration, SecurityContext context)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserDto> CreateUser(UserDto user)
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

    public AuthResponseDto GenerateToken(AuthRequestDto auth)
    {
        
        User user = _context.Users.Where(u => u.Username == auth.Username).First();

        var verify = BCrypt.Net.BCrypt.Verify(auth.Password, user.Password);

        if (verify)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
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
        
        var userPersist = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();

        if (userPersist != null) {
            throw new Exception("user already exists");
        }
    
        
    }

    #endregion

}
