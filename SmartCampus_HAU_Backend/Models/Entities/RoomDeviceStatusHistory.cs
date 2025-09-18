using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCampus_HAU_Backend.Models.Entities
{
    [Table("room_device_status_histories")]
    public class RoomDeviceStatusHistory
    {
        [Key]
        [Column("history_id")]
        public int HistoryId { get; set; }

        [Column("room_device_id")]
        [Required]
        public int RoomDeviceId { get; set; }

        [Column("device_type")]
        [StringLength(50)]
        [Required]
        public string DeviceType { get; set; } = null!;

        [Column("old_status")]
        public bool OldStatus { get; set; }

        [Column("new_status")]
        public bool NewStatus { get; set; }

        [Column("changed_at")]
        public DateTime ChangedAt { get; set; }

        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("quantity_affected")]
        [Required]
        public int QuantityAffected { get; set; }

        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [ForeignKey("RoomDeviceId")]
        public virtual RoomDevice RoomDevice { get; set; } = null!;

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } = null!;
    }
}
