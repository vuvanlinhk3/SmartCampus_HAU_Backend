namespace SmartCampus_HAU_Backend.Models.DTOs.Statistics
{
    public class DailyFailuresDTO
    {
        public DateTime Date { get; set; }
        public int TotalFailures { get; set; }
        public int TotalItemsAffected { get; set; }
        public List<UnitFailureDetailDTO> UnitFailures { get; set; } = new();
        public List<RoomDeviceFailureDetailDTO> RoomDeviceFailures { get; set; } = new();
        public List<RoomFailureSummaryDTO> FailuresByRoom { get; set; } = new();
    }

    public class UnitFailureDetailDTO
    {
        public int UnitId { get; set; }
        public string DeviceCode { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public DateTime FailedAt { get; set; }
        public string? Notes { get; set; }
    }

    public class RoomDeviceFailureDetailDTO
    {
        public int RoomDeviceId { get; set; }
        public string DeviceType { get; set; } = null!;
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public int QuantityAffected { get; set; }
        public DateTime FailedAt { get; set; }
        public string? Notes { get; set; }
    }

    public class RoomFailureSummaryDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int TotalFailures { get; set; }
        public int TotalItemsAffected { get; set; }
        public List<string> AffectedCategories { get; set; } = new();
    }

}
