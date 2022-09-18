using Security.Dtos;

namespace Security.Services;

public interface IAuthService
{
    public Task<UserDto> CreateUser(UserDto user);
    public AuthResponseDto GenerateToken(AuthRequestDto auth);
}
