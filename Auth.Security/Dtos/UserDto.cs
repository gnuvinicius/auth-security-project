using Security.Models;

namespace Security.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public IList<string> Roles { get; set; }

    public UserDto(User user)
    {
        this.Id = new Guid();
        this.Username = user.Username;
        this.Roles = new List<string> { user.Role };
    }
    
}
