using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("User")]
[Index("Username", Name = "User_username_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("userid")]
    public long Userid { get; set; }

    [Column("username")]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Column("passwordhash")]
    public byte[] Passwordhash { get; set; } = null!;

    [Column("displayname")]
    [StringLength(150)]
    public string Displayname { get; set; } = null!;

    [Column("email")]
    [StringLength(200)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(30)]
    public string? Phone { get; set; }

    [Column("lastloginutc")]
    public DateTime? Lastloginutc { get; set; }

    [Column("isactive")]
    public bool Isactive { get; set; } = true;

    
}