using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("users")]
public partial class User : IdentityUser
{
    [Column("user_id")]
    public override string Id { get; set; } = null!; // ID người dùng (GUID)

    [Column("user_name")]
    public override string? UserName { get; set; } // Tên đăng nhập

    [Column("email")]
    public override string? Email { get; set; } // Email người dùng

    [Column("phone_number")]
    public override string? PhoneNumber { get; set; } // Số điện thoại người dùng

    [Column("full_name")]
    [StringLength(255)]
    public string? FullName { get; set; } // Tên đầy đủ người dùng
}
