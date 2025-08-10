using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("rooms")]
public partial class Room
{
    [Key]
    [Column("room_id")]
    public int RoomId { get; set; }

    [Column("room_name")]
    [StringLength(50)]
    [Required]
    public string RoomName { get; set; } = null!;

    [Column("location")]
    [Required]
    public int Location { get; set; } // Tầng phòng: 1, 2, 3...

    [Column("room_type")]
    [StringLength(255)]
    [Required]
    public string RoomType { get; set; } = null!; // Phòng học (1), Phòng máy (2), Phòng riêng (3)

    [InverseProperty("Room")]
    public virtual ICollection<RoomDevice> RoomDevices { get; set; } = new List<RoomDevice>();

    [InverseProperty("Room")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();

    [InverseProperty("Room")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

