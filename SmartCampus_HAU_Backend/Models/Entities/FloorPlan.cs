using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.Entities
{
    [Table("floorplans")]
    public class FloorPlan
    {
        [Key]
        [Column("floorplan_id")]
        public int FloorPlanId { get; set; } // ID sơ đồ tầng

        [Column("floor_number")]
        [Required]
        public int FloorNumber { get; set; } // Số tầng

        [Column("image_url")]
        [StringLength(255)]
        [Required]
        public string ImageUrl { get; set; } = null!; // URL hình ảnh sơ đồ tầng
    }
}
