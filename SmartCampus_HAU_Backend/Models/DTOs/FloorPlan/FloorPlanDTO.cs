namespace SmartCampus_HAU_Backend.Models.DTOs.FloorPlan
{
    public class FloorPlanDTO
    {
        public int FloorPlanId { get; set; }
        public int FloorNumber { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
