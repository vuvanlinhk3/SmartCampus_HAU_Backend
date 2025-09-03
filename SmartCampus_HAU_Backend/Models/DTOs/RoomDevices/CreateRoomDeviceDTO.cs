namespace SmartCampus_HAU_Backend.Models.DTOs.RoomDevices
{
    public class CreateRoomDeviceDTO
    {
        public int RoomId { get; set; }
        public string? DeviceType { get; set; }
        public int Quantity { get; set; } 
        public bool Status { get; set; } 
        public string? Detail { get; set; } 
    }
}
