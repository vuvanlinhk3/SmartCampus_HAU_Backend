using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities;

[Table("bookings")]
public partial class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("room_id")]
    [Required]
    public int RoomId { get; set; }

    [Column("class_name")]
    [StringLength(100)]
    [Required]
    public string ClassName { get; set; } = null!;

    [Column("subject")]
    [StringLength(100)]
    [Required]
    public string Subject { get; set; } = null!;

    [Column("teacher")]
    [StringLength(100)]
    [Required]
    public string Teacher { get; set; } = null!;

    [Column("registered_by")]
    [StringLength(100)]
    [Required]
    public string RegisteredBy { get; set; } = null!;

    [Column("booking_date", TypeName = "date")]
    [Required]
    public DateTime BookingDate { get; set; }

    [Column("start_period")]
    [Required]
    public int? StartPeriod { get; set; }

    [Column("periods")]
    [Required]
    public int? Periods { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("Bookings")]
    public virtual Room Room { get; set; } = null!;
}

