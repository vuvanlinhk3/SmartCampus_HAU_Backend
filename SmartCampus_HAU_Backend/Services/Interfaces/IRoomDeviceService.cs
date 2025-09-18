using SmartCampus_HAU_Backend.Models.DTOs.RoomDevices;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IRoomDeviceService
    {
        Task<List<RoomDeviceDTO>> GetAllRoomDevicesAsync(int roomId); // Lấy tất cả thiết bị trong phòng
        Task<RoomDeviceDTO> GetRoomDeviceByIdAsync(int roomDeviceId); // Lấy thiết bị trong phòng theo ID
        Task<RoomDeviceDTO> AddRoomDeviceAsync(CreateRoomDeviceDTO createRoomDeviceDTO); // Tạo mới thiết bị trong phòng
        Task<RoomDeviceDTO> UpdateRoomDeviceAsync(int roomDeviceId, UpdateRoomDeviceDTO updateRoomDeviceDTO); // Cập nhật thiết bị trong phòng
        Task<RoomDeviceDTO> UpdateRoomDeviceStatusAsync(int roomDeviceId, int quantity, bool newStatus, string? notes = null); // Cập nhật trạng thái thiết bị trong phòng
        Task<bool> DeleteRoomDeviceAsync(int roomDeviceId); // Xóa thiết bị trong phòng
    }
}
