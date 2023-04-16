using Security.Dtos;

namespace Security.Services;

public interface IAuthService
{
    public Task<UserDto> CreateUser(AuthRequestDto user);
    public AuthResponseDto Authenticate(AuthRequestDto auth);
}
