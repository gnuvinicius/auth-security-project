using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Models;

[Table("tb_user")]
public class User
{
    [Column("user_id")]
    public int Id { get; set; }

    [Column("username")]
    public string? Username { get; set; }

    [Column("password")]
    public string? Password { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("role")]
    public string? Role { get; set; }
}
