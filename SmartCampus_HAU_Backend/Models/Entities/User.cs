using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("users")]
public partial class User : IdentityUser
{
    [Column("user_id")]
    public override string Id { get; set; } = null!;

    [Column("user_name")]
    public override string? UserName { get; set; }

    [Column("email")]
    public override string? Email { get; set; }

    [Column("phone_number")]
    public override string? PhoneNumber { get; set; }

    [Column("full_name")]
    [StringLength(255)]
    public string FullName { get; set; } = null!;
}
