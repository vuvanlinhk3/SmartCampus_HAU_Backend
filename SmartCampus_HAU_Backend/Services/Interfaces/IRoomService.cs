using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Models.DTOs.Rooms;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<AllRoomWithStatusDTO>> GetAllRoomsWithStatus(); // Lấy tất cả phòng với trạng thái hiện tại
        Task<RoomDetailDTO> AddRoomAsync(CreateRoomDTO createRoomDTO); // Tạo mới phòng
        Task<RoomDetailDTO> GetRoomByNameAsync(string roomName); // Lấy phòng theo tên
        Task<RoomDetailDTO> UpdateRoomAsync(int roomId, RoomDetailDTO roomDetailDTO); // Cập nhật phòng
        Task<bool> DeleteRoomAsync(int roomId); // Xóa phòng
    }
}
