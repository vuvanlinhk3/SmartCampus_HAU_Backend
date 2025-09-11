namespace SmartCampus_HAU_Backend.Models.DTOs.Rooms
{
    public class RoomDeviceListDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public List<DeviceItemDTO> RoomDevices { get; set; } = new();
        public List<DeviceItemDTO> Units { get; set; } = new();
    }

    public class DeviceItemDTO
    {
        public int DeviceItemId { get; set; }
        public string DeviceType { get; set; } = null!;
        public int? Quantity { get; set; }
        public bool Status { get; set; }
        public string? Detail { get; set; }
        public string Category { get; set; } = null!; 
    }
}
