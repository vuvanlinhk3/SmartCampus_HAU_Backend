using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Statistics;
using SmartCampus_HAU_Backend.Data;
using SmartCampus_HAU_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class RoomDeviceStatisticsService : IRoomDeviceStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public RoomDeviceStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FrequencyStatisticsDTO> GetRoomDeviceFrequencyStatisticsAsync()
        {
            var now = DateTime.Today;

            return new FrequencyStatisticsDTO
            {
                Weekly = await GetRoomDevicePeriodStatsAsync(now.AddDays(-7), now),
                Monthly = await GetRoomDevicePeriodStatsAsync(now.AddDays(-30), now),
                Yearly = await GetRoomDevicePeriodStatsAsync(now.AddDays(-365), now),
                AverageFrequency = await GetRoomDeviceAverageFrequencyAsync()
            };
        }

        public async Task<DailyFailuresDTO> GetRoomDeviceDailyFailuresAsync(DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Today;
            var startDate = targetDate.Date;
            var endDate = startDate.AddDays(1);

            var roomDeviceFailures = await _context.RoomDeviceStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .Join(_context.RoomDevices,
                      history => history.RoomDeviceId,
                      roomDevice => roomDevice.RoomDeviceId,
                      (history, roomDevice) => new { History = history, RoomDevice = roomDevice })
                .Join(_context.Rooms,
                      x => x.RoomDevice.RoomId,
                      room => room.RoomId,
                      (x, room) => new { x.History, x.RoomDevice, Room = room })
                .Select(x => new RoomDeviceFailureDetailDTO
                {
                    RoomDeviceId = x.RoomDevice.RoomDeviceId,
                    DeviceType = x.History.DeviceType, 
                    RoomId = x.RoomDevice.RoomId,
                    RoomName = x.Room.RoomName,
                    TotalQuantity = x.RoomDevice.Quantity,
                    QuantityAffected = x.History.QuantityAffected,
                    FailedAt = x.History.ChangedAt,
                    Notes = x.History.Notes
                })
                .OrderByDescending(x => x.FailedAt)
                .ToListAsync();

            var totalFailures = roomDeviceFailures.Count;
            var totalItemsAffected = roomDeviceFailures.Sum(x => x.QuantityAffected);

            var failuresByRoom = roomDeviceFailures
                .GroupBy(f => new { f.RoomId, f.RoomName })
                .Select(g => new RoomFailureSummaryDTO
                {
                    RoomId = g.Key.RoomId,
                    RoomName = g.Key.RoomName,
                    TotalFailures = g.Count(),
                    TotalItemsAffected = g.Sum(x => x.QuantityAffected),
                    AffectedCategories = g.Select(x => x.DeviceType).Distinct().ToList() 
                })
                .ToList();

            return new DailyFailuresDTO
            {
                Date = targetDate,
                TotalFailures = totalFailures,
                TotalItemsAffected = totalItemsAffected,
                UnitFailures = new(), 
                RoomDeviceFailures = roomDeviceFailures,
                FailuresByRoom = failuresByRoom
            };
        }

        public async Task<PeriodFailureStatsDTO> GetRoomDevicePeriodStatsAsync(DateTime startDate, DateTime endDate)
        {
            var failures = await _context.RoomDeviceStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .ToListAsync();

            var totalDays = (endDate - startDate).Days;
            var averagePerDay = totalDays > 0 ? (double)failures.Count / totalDays : 0;
            var totalItemsAffected = failures.Sum(h => h.QuantityAffected);

            var failuresByCategory = failures
                .GroupBy(h => h.DeviceType) 
                .Select(g => new DeviceTypeFailureDTO 
                {
                    DeviceType = g.Key, 
                    FailureCount = g.Count(),
                    ItemsAffected = g.Sum(x => x.QuantityAffected)
                })
                .ToList();

            var dailyBreakdown = failures
                .GroupBy(h => h.ChangedAt.Date)
                .Select(g => new DailyFailureDTO
                {
                    Date = g.Key,
                    FailureCount = g.Count(),
                    ItemsAffected = g.Sum(x => x.QuantityAffected)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new PeriodFailureStatsDTO
            {
                TotalFailures = failures.Count,
                TotalItemsAffected = totalItemsAffected,
                AverageFailuresPerDay = Math.Round(averagePerDay, 2),
                PeriodStart = startDate,
                PeriodEnd = endDate,
                FailuresByCategory = failuresByCategory,
                DailyBreakdown = dailyBreakdown
            };
        }

        public async Task<AverageFrequencyDTO> GetRoomDeviceAverageFrequencyAsync()
        {
            var daysToAnalyze = 365;
            var startDate = DateTime.Today.AddDays(-daysToAnalyze);
            var endDate = DateTime.Today;

            var failures = await _context.RoomDeviceStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .ToListAsync();

            var totalFailures = failures.Count;
            var totalItemsAffected = failures.Sum(h => h.QuantityAffected);
            var averageDaysBetween = totalFailures > 0 ? (double)daysToAnalyze / totalFailures : 0;
            var averagePerWeek = totalFailures > 0 ? (double)totalFailures * 7 / daysToAnalyze : 0;
            var averageItemsPerFailure = totalFailures > 0 ? (double)totalItemsAffected / totalFailures : 0;

            return new AverageFrequencyDTO
            {
                AverageDaysBetweenFailures = Math.Round(averageDaysBetween, 2),
                AverageFailuresPerWeek = Math.Round(averagePerWeek, 2),
                AverageItemsAffectedPerFailure = Math.Round(averageItemsPerFailure, 2),
                DaysAnalyzed = daysToAnalyze,
                FrequencyDescription = GetFrequencyDescription(averageDaysBetween)
            };
        }

        private static string GetFrequencyDescription(double averageDays)
        {
            return averageDays switch
            {
                <= 1 => "Rất cao - hàng ngày có RoomDevice hỏng",
                <= 3 => "Cao - cứ 2-3 ngày có RoomDevice hỏng",
                <= 7 => "Trung bình cao - hàng tuần có RoomDevice hỏng",
                <= 30 => "Trung bình - hàng tháng có RoomDevice hỏng",
                <= 90 => "Thấp - hàng quý có RoomDevice hỏng",
                _ => "Rất thấp - hiếm khi có RoomDevice hỏng"
            };
        }
    }
}
