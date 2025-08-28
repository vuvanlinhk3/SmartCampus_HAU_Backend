using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("bookings")]
public partial class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; } // ID đặt phòng

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; } // ID phòng

    [Column("class_name")]
    [StringLength(100)]
    [Required]
    public string ClassName { get; set; } = null!; // Tên lớp học

    [Column("subject")]
    [StringLength(100)]
    [Required]
    public string Subject { get; set; } = null!; // Tên môn học

    [Column("teacher")]
    [StringLength(100)]
    [Required]
    public string Teacher { get; set; } = null!; // Tên giảng viên

    [Column("registered_by")]
    [StringLength(100)]
    [Required]
    public string RegisteredBy { get; set; } = null!; // Tên người đăng ký

    [Column("booking_date", TypeName = "date")]
    [Required]
    public DateTime BookingDate { get; set; } // Ngày đặt phòng

    [Column("start_period")]
    [Required]
    public int? StartPeriod { get; set; } // Tiết bắt đầu

    [Column("periods")]
    [Required]
    public int? Periods { get; set; } // Số tiết

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } // Thời gian tạo

    [ForeignKey("RoomId")]
    [InverseProperty("Bookings")]
    public virtual Room Room { get; set; } = null!; // Thông tin chi tiết về phòng được đặt
}

