using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("units")]
public partial class Unit
{
    [Key]
    [Column("unit_id")]
    public int UnitId { get; set; }

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; }

    [Column("device_type")]
    [StringLength(50)]
    [Required]
    public string DeviceType { get; set; } = null!;

    [Column("unit_label")]
    [StringLength(50)]
    public string? UnitLabel { get; set; }

    [Column("device_code")]
    [StringLength(100)]
    public string? DeviceCode { get; set; }

    [Column("status")]
    public bool Status { get; set; }

    [Column("detail")]
    [StringLength(255)]
    [Required]
    public string Detail { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("Units")]
    public virtual Room Room { get; set; } = null!;
}

