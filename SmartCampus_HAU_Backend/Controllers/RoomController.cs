using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoomsWithStatus()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsWithStatus();
                return new OkObjectResult(rooms);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO createRoomDTO)
        {
            if (createRoomDTO == null)
            {
                return new BadRequestObjectResult("Invalid room data.");
            }
            try
            {
                var result = await _roomService.CreateRoomAsync(createRoomDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);

            }
        }

        [HttpGet("{roomName}")]
        public async Task<IActionResult> GetRoomByName(string roomName)
        {
            try
            {
                var room = await _roomService.GetRoomByNameAsync(roomName);
                return new OkObjectResult(room);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("update/{roomId}")]
        public async Task<IActionResult> UpdateRoom([FromBody] int roomId, RoomDetailDTO roomDetailDTO)
        {
            if (roomId <= 0 || roomDetailDTO == null)
            {
                return new BadRequestObjectResult("Invalid room data.");
            }
            try
            {
                var updatedRoom = await _roomService.UpdateRoomAsync(roomId, roomDetailDTO);
                return new OkObjectResult(updatedRoom);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("delete/{roomId}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            if (roomId <= 0)
            {
                return new BadRequestObjectResult("Invalid room ID.");
            }
            try
            {
                var result = await _roomService.DeleteRoomAsync(roomId);
                return new OkObjectResult("Room deleted successfully.");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

    }
}