using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Models.DTOs.Rooms
{
    public class UpdateRoomDetailDTO
    {
        public string? RoomName { get; set; }
        public int Location { get; set; } // Tầng phòng: 1, 2, 3...
        public string? RoomType { get; set; } // Phòng học (1), Phòng máy (2), Phòng riêng (3)
    }
}
