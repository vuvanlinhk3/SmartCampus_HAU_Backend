namespace SmartCampus_HAU_Backend.Models.DTOs.Statistics
{
    public class UnitStatisticsDTO
    {
        public UnitStatisticsByPeriodDTO Daily { get; set; } = new();
        public UnitStatisticsByPeriodDTO Monthly { get; set; } = new();
        public UnitStatisticsByPeriodDTO Yearly { get; set; } = new();
        public List<UnitTypeFailureDTO> FailuresByType { get; set; } = new();
        public UnitFailureFrequencyDTO FailureFrequency { get; set; } = new();
    }

    public class UnitStatisticsByPeriodDTO
    {
        public int TotalFailures { get; set; }
        public int TotalRepairs { get; set; }
        public List<UnitTypeFailureDTO> FailuresByType { get; set; } = new();
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }

    public class UnitTypeFailureDTO
    {
        public string DeviceType { get; set; } = null!;
        public int FailureCount { get; set; }
        public int RepairCount { get; set; }
        public double FailureRate { get; set; }
        public List<UnitRoomFailureDTO> RoomBreakdown { get; set; } = new();
        public List<FailedUnitDetailDTO> FailedUnits { get; set; } = new();
    }

    public class UnitRoomFailureDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int FailureCount { get; set; }
    }

    public class FailedUnitDetailDTO
    {
        public int UnitId { get; set; }
        public string? DeviceCode { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public DateTime LastFailureDate { get; set; }
        public string? Notes { get; set; }
    }

    public class UnitFailureFrequencyDTO
    {
        public double AverageDaysBetweenFailures { get; set; }
        public int TotalFailuresInPeriod { get; set; }
        public int DaysAnalyzed { get; set; }
        public string FrequencyDescription { get; set; } = null!;
        public int TotalUnitsTracked { get; set; }
    }

}
