using SmartCampus_HAU_Backend.Models.DTOs.Statistics;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUnitStatisticsService
    {
        Task<FrequencyStatisticsDTO> GetUnitFrequencyStatisticsAsync();
        Task<DailyFailuresDTO> GetUnitDailyFailuresAsync(DateTime? date = null);
        Task<PeriodFailureStatsDTO> GetUnitPeriodStatsAsync(DateTime startDate, DateTime endDate);
        Task<AverageFrequencyDTO> GetUnitAverageFrequencyAsync();
    }
}
