using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("room_devices")]
public partial class RoomDevice
{
    [Key]
    [Column("room_device_id")]
    public int RoomDeviceId { get; set; } // ID thiết bị

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; } // ID phòng chứa thiết bị

    [Column("device_type")]
    [StringLength(50)]
    [Required]
    public string DeviceType { get; set; } = null!; // Loại thiết bị: Quạt (quat), Máy chiếu (maychieu), Bóng đèn (bongden)
    
    [Column("quantity")]
    [Required]
    public int Quantity { get; set; } // Số lượng thiết bị trong phòng

    [Column("status")]
    public bool Status { get; set; } // Trạng thái thiết bị: Hoạt động (true), Hỏng (false)

    [Column("detail")]
    [StringLength(255)]
    [Required]
    public string Detail { get; set; } = null!; // Mô tả chi tiết về thiết bị

    [ForeignKey("RoomId")]
    [InverseProperty("RoomDevices")]
    public virtual Room Room { get; set; } = null!; // Phòng chứa thiết bị
}

