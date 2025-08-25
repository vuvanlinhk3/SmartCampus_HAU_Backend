using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Models.DTOs.Rooms
{
    public class RoomDetailDTO
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int Location { get; set; } // Tầng phòng: 1, 2, 3...
        public string? RoomType { get; set; } // Phòng học (1), Phòng máy (2), Phòng riêng (3)
        //public bool IsAvailable { get; set; } = true; // Mặc định là có sẵn
        public virtual List<RoomDevice> RoomDevices { get; set; } = new List<RoomDevice>(); // Danh sách ID thiết bị trong phòng
        public virtual List<Unit> Units { get; set; } = new List<Unit>(); // Danh sách ID thiết bị quản lí theo đơn vị
        public virtual List<Booking> Bookings { get; set; } = new List<Booking>(); // Danh sách ID đặt phòng
    }
}
