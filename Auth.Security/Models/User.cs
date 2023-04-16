using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Models;

[Table("tb_user")]
public class User
{
    [Key]
    [Column("user_id")]
    public Guid Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("role")]
    public string Role { get; set; }

    public User(string username, string password, string role)
    {
        this.Id = Guid.NewGuid();
        this.Username = username;
        this.Password = password;
        this.Role = role;
        CreatedAt = DateTime.Now;
    }
}
