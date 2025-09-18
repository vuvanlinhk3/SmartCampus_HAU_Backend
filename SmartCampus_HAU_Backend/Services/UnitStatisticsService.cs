using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Statistics;
using SmartCampus_HAU_Backend.Data;
using SmartCampus_HAU_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class UnitStatisticsService : IUnitStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public UnitStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FrequencyStatisticsDTO> GetUnitFrequencyStatisticsAsync()
        {
            var now = DateTime.Today;

            return new FrequencyStatisticsDTO
            {
                Weekly = await GetUnitPeriodStatsAsync(now.AddDays(-7), now),
                Monthly = await GetUnitPeriodStatsAsync(now.AddDays(-30), now),
                Yearly = await GetUnitPeriodStatsAsync(now.AddDays(-365), now),
                AverageFrequency = await GetUnitAverageFrequencyAsync()
            };
        }

        public async Task<DailyFailuresDTO> GetUnitDailyFailuresAsync(DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Today;
            var startDate = targetDate.Date;
            var endDate = startDate.AddDays(1);

            var unitFailures = await _context.UnitStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .Join(_context.Units,
                      history => history.UnitId,
                      unit => unit.UnitId,
                      (history, unit) => new { History = history, Unit = unit })
                .Join(_context.Rooms,
                      x => x.Unit.RoomId,
                      room => room.RoomId,
                      (x, room) => new { x.History, x.Unit, Room = room })
                .Select(x => new UnitFailureDetailDTO
                {
                    UnitId = x.Unit.UnitId,
                    DeviceCode = x.Unit.DeviceCode,
                    DeviceType = x.History.DeviceType,
                    RoomId = x.Unit.RoomId,
                    RoomName = x.Room.RoomName,
                    FailedAt = x.History.ChangedAt,
                    Notes = x.History.Notes
                })
                .OrderByDescending(x => x.FailedAt)
                .ToListAsync();

            var failuresByRoom = unitFailures
                .GroupBy(f => new { f.RoomId, f.RoomName })
                .Select(g => new RoomFailureSummaryDTO
                {
                    RoomId = g.Key.RoomId,
                    RoomName = g.Key.RoomName,
                    TotalFailures = g.Count(),
                    TotalItemsAffected = g.Count(), 
                    AffectedCategories = g.Select(x => x.DeviceType).Distinct().ToList() 
                })
                .ToList();

            return new DailyFailuresDTO
            {
                Date = targetDate,
                TotalFailures = unitFailures.Count,
                TotalItemsAffected = unitFailures.Count,
                UnitFailures = unitFailures,
                RoomDeviceFailures = new(), 
                FailuresByRoom = failuresByRoom
            };
        }

        public async Task<PeriodFailureStatsDTO> GetUnitPeriodStatsAsync(DateTime startDate, DateTime endDate)
        {
            var failures = await _context.UnitStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .ToListAsync();

            var totalDays = (endDate - startDate).Days;
            var averagePerDay = totalDays > 0 ? (double)failures.Count / totalDays : 0;

            var failuresByCategory = failures
                .GroupBy(h => h.DeviceType) 
                .Select(g => new DeviceTypeFailureDTO 
                {
                    DeviceType = g.Key, 
                    FailureCount = g.Count(),
                    ItemsAffected = g.Count() 
                })
                .ToList();

            var dailyBreakdown = failures
                .GroupBy(h => h.ChangedAt.Date)
                .Select(g => new DailyFailureDTO
                {
                    Date = g.Key,
                    FailureCount = g.Count(),
                    ItemsAffected = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new PeriodFailureStatsDTO
            {
                TotalFailures = failures.Count,
                TotalItemsAffected = failures.Count,
                AverageFailuresPerDay = Math.Round(averagePerDay, 2),
                PeriodStart = startDate,
                PeriodEnd = endDate,
                FailuresByCategory = failuresByCategory,
                DailyBreakdown = dailyBreakdown
            };
        }

        public async Task<AverageFrequencyDTO> GetUnitAverageFrequencyAsync()
        {
            var daysToAnalyze = 365;
            var startDate = DateTime.Today.AddDays(-daysToAnalyze);
            var endDate = DateTime.Today;

            var failures = await _context.UnitStatusHistories
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt < endDate &&
                           h.OldStatus == true && h.NewStatus == false)
                .ToListAsync();

            var totalFailures = failures.Count;
            var averageDaysBetween = totalFailures > 0 ? (double)daysToAnalyze / totalFailures : 0;
            var averagePerWeek = totalFailures > 0 ? (double)totalFailures * 7 / daysToAnalyze : 0;
            var averageItemsPerFailure = 1.0;

            return new AverageFrequencyDTO
            {
                AverageDaysBetweenFailures = Math.Round(averageDaysBetween, 2),
                AverageFailuresPerWeek = Math.Round(averagePerWeek, 2),
                AverageItemsAffectedPerFailure = averageItemsPerFailure,
                DaysAnalyzed = daysToAnalyze,
                FrequencyDescription = GetFrequencyDescription(averageDaysBetween)
            };
        }

        private static string GetFrequencyDescription(double averageDays)
        {
            return averageDays switch
            {
                <= 1 => "Rất cao - hàng ngày có Unit hỏng",
                <= 3 => "Cao - cứ 2-3 ngày có Unit hỏng",
                <= 7 => "Trung bình cao - hàng tuần có Unit hỏng",
                <= 30 => "Trung bình - hàng tháng có Unit hỏng",
                <= 90 => "Thấp - hàng quý có Unit hỏng",
                _ => "Rất thấp - hiếm khi có Unit hỏng"
            };
        }
    }
}
