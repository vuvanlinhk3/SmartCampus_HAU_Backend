using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("rooms")]
public partial class Room
{
    [Key]
    [Column("room_id")]
    public int RoomId { get; set; } // ID phòng

    [Column("room_name")]
    [StringLength(50)]
    [Required]
    public string RoomName { get; set; } = null!; // Tên phòng: H505, H602...

    [Column("location")]
    [Required]
    public int Location { get; set; } // Tầng phòng: 1, 2, 3...

    [Column("room_type")]
    [StringLength(255)]
    [Required]
    public string RoomType { get; set; } = null!; // Phòng học (hoc), Phòng máy (may), Phòng riêng (rieng)

    [InverseProperty("Room")]
    public virtual ICollection<RoomDevice> RoomDevices { get; set; } = new List<RoomDevice>(); // Danh sách thiết bị trong phòng

    [InverseProperty("Room")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>(); // Danh sách thiết bị quản lí theo đơn vị

    [InverseProperty("Room")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Danh sách đặt phòng
}

