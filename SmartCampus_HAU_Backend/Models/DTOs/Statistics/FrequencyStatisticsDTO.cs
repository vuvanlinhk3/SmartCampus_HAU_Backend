namespace SmartCampus_HAU_Backend.Models.DTOs.Statistics
{
    public class FrequencyStatisticsDTO
    {
        public PeriodFailureStatsDTO Weekly { get; set; } = new();
        public PeriodFailureStatsDTO Monthly { get; set; } = new();
        public PeriodFailureStatsDTO Yearly { get; set; } = new();
        public AverageFrequencyDTO AverageFrequency { get; set; } = new();
    }

    public class PeriodFailureStatsDTO
    {
        public int TotalFailures { get; set; }
        public int TotalItemsAffected { get; set; }
        public double AverageFailuresPerDay { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<DeviceTypeFailureDTO> FailuresByCategory { get; set; } = new();
        public List<DailyFailureDTO> DailyBreakdown { get; set; } = new();
    }

    public class DeviceTypeFailureDTO
    {
        public string DeviceType { get; set; } = null!;
        public int FailureCount { get; set; }
        public int ItemsAffected { get; set; }
    }

    public class DailyFailureDTO
    {
        public DateTime Date { get; set; }
        public int FailureCount { get; set; }
        public int ItemsAffected { get; set; }
    }

    public class AverageFrequencyDTO
    {
        public double AverageDaysBetweenFailures { get; set; }
        public double AverageFailuresPerWeek { get; set; }
        public double AverageItemsAffectedPerFailure { get; set; }
        public int DaysAnalyzed { get; set; }
        public string FrequencyDescription { get; set; } = null!;
    }

}
