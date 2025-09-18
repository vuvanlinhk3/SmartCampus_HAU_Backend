// Controllers/UnitStatisticsController.cs
using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Statistics;
using Microsoft.AspNetCore.Authorization;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitStatisticsController : ControllerBase
    {
        private readonly IUnitStatisticsService _unitStatisticsService;

        public UnitStatisticsController(IUnitStatisticsService unitStatisticsService)
        {
            _unitStatisticsService = unitStatisticsService;
        }

        [HttpGet("frequency")]
        public async Task<IActionResult> GetUnitFrequencyStatistics()
        {
            try
            {
                var stats = await _unitStatisticsService.GetUnitFrequencyStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi thống kê tần suất Unit", details = ex.Message });
            }
        }

        [HttpGet("daily-failures")]
        public async Task<IActionResult> GetUnitDailyFailures([FromQuery] DateTime? date)
        {
            try
            {
                var failures = await _unitStatisticsService.GetUnitDailyFailuresAsync(date);
                return Ok(failures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi thống kê Unit hỏng trong ngày", details = ex.Message });
            }
        }
    }
}
