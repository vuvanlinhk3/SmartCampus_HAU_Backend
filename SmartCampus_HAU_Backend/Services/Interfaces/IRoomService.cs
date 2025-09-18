using SmartCampus_HAU_Backend.Models.DTOs.Rooms;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<AllRoomWithStatusDTO>> GetAllRoomsWithStatus(); // Lấy tất cả phòng với trạng thái hiện tại
        Task<RoomDeviceListDTO> GetAllDevicesInRoomAsync(int roomId); // Lấy tất cả thiết bị trong phòng (RoomDevice+Unit)
        Task<List<RoomDeviceListDTO>> GetAllDevicesAllRoomsAsync(); // Lấy toàn bộ các thiết bị (RoomDevice+Unit), nhóm theo phòng
        Task<RoomDetailDTO> AddRoomAsync(CreateRoomDTO createRoomDTO); // Tạo mới phòng
        Task<RoomDetailDTO> GetRoomByNameAsync(string roomName); // Lấy phòng theo tên
        Task<RoomDetailDTO> UpdateRoomAsync(int roomId, UpdateRoomDetailDTO updateRoomDetailDTO); // Cập nhật phòng
        Task<bool> DeleteRoomAsync(int roomId); // Xóa phòng
    }
}
