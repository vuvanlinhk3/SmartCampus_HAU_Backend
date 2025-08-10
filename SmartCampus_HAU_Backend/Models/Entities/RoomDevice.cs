using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("room_devices")]
public partial class RoomDevice
{
    [Key]
    [Column("room_device_id")]
    public int RoomDeviceId { get; set; }

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; }

    [Column("device_type")]
    [StringLength(50)]
    [Required]
    public string DeviceType { get; set; } = null!;

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; }

    [Column("status")]
    public bool Status { get; set; }

    [Column("detail")]
    [StringLength(255)]
    [Required]
    public string Detail { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("RoomDevices")]
    public virtual Room Room { get; set; } = null!;
}

