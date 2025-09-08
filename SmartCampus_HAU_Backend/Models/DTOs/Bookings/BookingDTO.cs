namespace SmartCampus_HAU_Backend.Models.DTOs.Bookings
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string? ClassName { get; set; }
        public string Subject { get; set; } = null!;
        public string? Teacher { get; set; }
        public string RegisteredBy { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public int? StartPeriod { get; set; }
        public int? Periods { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
