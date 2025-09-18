// Controllers/RoomDeviceStatisticsController.cs
using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Statistics;
using Microsoft.AspNetCore.Authorization;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomDeviceStatisticsController : ControllerBase
    {
        private readonly IRoomDeviceStatisticsService _roomDeviceStatisticsService;

        public RoomDeviceStatisticsController(IRoomDeviceStatisticsService roomDeviceStatisticsService)
        {
            _roomDeviceStatisticsService = roomDeviceStatisticsService;
        }

        [HttpGet("frequency")]
        public async Task<IActionResult> GetRoomDeviceFrequencyStatistics()
        {
            try
            {
                var stats = await _roomDeviceStatisticsService.GetRoomDeviceFrequencyStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi thống kê tần suất RoomDevice", details = ex.Message });
            }
        }

        [HttpGet("daily-failures")]
        public async Task<IActionResult> GetRoomDeviceDailyFailures([FromQuery] DateTime? date)
        {
            try
            {
                var failures = await _roomDeviceStatisticsService.GetRoomDeviceDailyFailuresAsync(date);
                return Ok(failures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi thống kê RoomDevice hỏng trong ngày", details = ex.Message });
            }
        }
    }
}