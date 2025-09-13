namespace SmartCampus_HAU_Backend.Models.DTOs.Units
{
    public class CreateUnitDTO
    {
        public int RoomId { get; set; } 
        public string DeviceType { get; set; } = null!;
        public string? DeviceCode { get; set; }
        public bool Status { get; set; } 
        public string Detail { get; set; } = null!;
    }
}
