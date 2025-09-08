using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("units")]
public partial class Unit
{
    [Key]
    [Column("unit_id")]
    public int UnitId { get; set; } // ID thiết bị quản lí theo đơn vị

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; } // ID phòng chứa thiết bị

    [Column("device_type")]
    [StringLength(50)]
    [Required]
    public string DeviceType { get; set; } = null!; // Loại thiết bị: Máy lạnh (maylanh)...

    [Column("device_code")]
    [StringLength(100)]
    public string? DeviceCode { get; set; } // Mã thiết bị: S/N, Mã vạch...

    [Column("status")]
    public bool Status { get; set; } = true;// Trạng thái thiết bị: Hoạt động (true), Hỏng (false)

    [Column("detail")]
    [StringLength(255)]
    [Required]
    public string Detail { get; set; } = null!; // Mô tả chi tiết về thiết bị (vị trí: gần cửa, ...)

    [ForeignKey("RoomId")]
    [InverseProperty("Units")]
    public virtual Room Room { get; set; } = null!; // Phòng chứa thiết bị
}

