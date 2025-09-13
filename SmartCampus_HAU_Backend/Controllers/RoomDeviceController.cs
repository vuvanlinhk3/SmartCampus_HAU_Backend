using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.RoomDevices;
using Microsoft.AspNetCore.Mvc;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomDeviceController : ControllerBase
    {
        private readonly IRoomDeviceService _roomDeviceService;
        public RoomDeviceController(IRoomDeviceService roomDeviceService)
        {
            _roomDeviceService = roomDeviceService;
        }

        [HttpGet("room/device/getall/{roomId}")]
        public async Task<IActionResult> GetAllRoomDevices([FromRoute] int roomId)
        {
            try
            {
                var devices = await _roomDeviceService.GetAllRoomDevicesAsync(roomId);
                return new OkObjectResult(devices);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("room/device/get/{roomDeviceId}")]
        public async Task<IActionResult> GetRoomDevicesById([FromRoute] int roomDeviceId)
        {
            try
            {
                var devices = await _roomDeviceService.GetRoomDeviceByIdAsync(roomDeviceId);
                return new OkObjectResult(devices);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("room/device/add")]
        public async Task<IActionResult> AddRoomDevice([FromBody] CreateRoomDeviceDTO createRoomDeviceDTO)
        {
            if (createRoomDeviceDTO == null)
            {
                return new BadRequestObjectResult("Invalid device data.");
            }
            try
            {
                var result = await _roomDeviceService.AddRoomDeviceAsync(createRoomDeviceDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("room/device/update/{roomDeviceId}")]
        public async Task<IActionResult> UpdateRoomDevice([FromRoute] int roomDeviceId, [FromBody] UpdateRoomDeviceDTO updateRoomDeviceDTO)
        {
            if (updateRoomDeviceDTO == null)
            {
                return new BadRequestObjectResult("Invalid device data.");
            }
            try
            {
                var updatedDevice = await _roomDeviceService.UpdateRoomDeviceAsync(roomDeviceId, updateRoomDeviceDTO);
                return new OkObjectResult(updatedDevice);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("room/device/delete/{roomDeviceId}")]
        public async Task<IActionResult> DeleteRoomDevice([FromRoute] int roomDeviceId)
        {
            try
            {
                var result = await _roomDeviceService.DeleteRoomDeviceAsync(roomDeviceId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
