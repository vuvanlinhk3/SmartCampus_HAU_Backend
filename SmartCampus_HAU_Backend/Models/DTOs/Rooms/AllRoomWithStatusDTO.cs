namespace SmartCampus_HAU_Backend.Models.DTOs.Rooms
{
    public class AllRoomWithStatusDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int Location { get; set; }
        public string Status { get; set; } = null!; // "Đang học" hoặc "Trống"
        public string? Subject { get; set; }
        public string? Teacher { get; set; }
        public string? TimeRange { get; set; } // Khoảng thời gian học (vd: "07:30 - 09:10")
        public DateTime BookingDate { get; set; }
    }
}

