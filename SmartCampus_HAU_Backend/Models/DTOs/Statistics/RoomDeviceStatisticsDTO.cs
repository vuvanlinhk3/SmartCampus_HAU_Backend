namespace SmartCampus_HAU_Backend.Models.DTOs.Statistics
{
    public class RoomDeviceStatisticsDTO
    {
        public RoomDeviceStatisticsByPeriodDTO Daily { get; set; } = new();
        public RoomDeviceStatisticsByPeriodDTO Monthly { get; set; } = new();
        public RoomDeviceStatisticsByPeriodDTO Yearly { get; set; } = new();
        public List<RoomDeviceTypeFailureDTO> FailuresByType { get; set; } = new();
        public RoomDeviceFailureFrequencyDTO FailureFrequency { get; set; } = new();
    }

    public class RoomDeviceStatisticsByPeriodDTO
    {
        public int TotalFailures { get; set; }
        public int TotalRepairs { get; set; } 
        public List<RoomDeviceTypeFailureDTO> FailuresByType { get; set; } = new();
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }

    public class RoomDeviceTypeFailureDTO
    {
        public string DeviceType { get; set; } = null!;
        public int FailureCount { get; set; }
        public int RepairCount { get; set; } 
        public double FailureRate { get; set; }
        public List<RoomDeviceRoomFailureDTO> RoomBreakdown { get; set; } = new();
        public List<FailedRoomDeviceDetailDTO> FailedRoomDevices { get; set; } = new();
    }

    public class RoomDeviceRoomFailureDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int FailureCount { get; set; }
        public int RoomDeviceGroupsAffected { get; set; } 
    }

    public class FailedRoomDeviceDetailDTO
    {
        public int RoomDeviceId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime LastFailureDate { get; set; }
        public string? Notes { get; set; }
    }

    public class RoomDeviceFailureFrequencyDTO
    {
        public double AverageDaysBetweenFailures { get; set; }
        public int TotalFailuresInPeriod { get; set; }
        public int DaysAnalyzed { get; set; }
        public string FrequencyDescription { get; set; } = null!;
        public int TotalRoomDeviceItemsTracked { get; set; } 
        public int TotalRoomDeviceGroupsTracked { get; set; }
    }

}
