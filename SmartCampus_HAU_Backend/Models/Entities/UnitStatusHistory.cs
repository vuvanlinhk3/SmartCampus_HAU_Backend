using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities
{
    [Table("unit_status_histories")]
    public class UnitStatusHistory
    {
        [Key]
        [Column("history_id")]
        public int HistoryId { get; set; }

        [Column("unit_id")]
        [Required]
        public int UnitId { get; set; }

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

        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; } = null!;

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } = null!;
    }
}
