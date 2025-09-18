using SmartCampus_HAU_Backend.Models.DTOs.Statistics;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IRoomDeviceStatisticsService
    {
        Task<FrequencyStatisticsDTO> GetRoomDeviceFrequencyStatisticsAsync();
        Task<DailyFailuresDTO> GetRoomDeviceDailyFailuresAsync(DateTime? date = null);
        Task<PeriodFailureStatsDTO> GetRoomDevicePeriodStatsAsync(DateTime startDate, DateTime endDate);
        Task<AverageFrequencyDTO> GetRoomDeviceAverageFrequencyAsync();
    }
}
