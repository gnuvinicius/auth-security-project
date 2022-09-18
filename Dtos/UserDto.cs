namespace Security.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public IList<string>? Roles { get; set; }
    
}
