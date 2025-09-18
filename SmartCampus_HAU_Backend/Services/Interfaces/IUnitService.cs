using SmartCampus_HAU_Backend.Models.DTOs.Units;
using static Azure.Core.HttpHeader;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUnitService
    {
        Task<List<UnitDTO>> GetAllUnitsAsync(int roomId); // Lấy tất cả thiết bị quản lí theo đơn vị trong phòng
        Task<UnitDTO> AddUnitAsync(CreateUnitDTO createUnitDTO); // Tạo mới thiết bị quản lí theo đơn vị
        Task<UnitDTO> GetUnitByIdAsync(int unitId); // Lấy thiết bị quản lí theo đơn vị theo ID
        Task<UnitDTO> UpdateUnitAsync(int unitId, UpdateUnitDTO updateUnitDTO); // Cập nhật thiết bị quản lí theo đơn vị
        Task<UnitDTO> UpdateUnitStatusAsync(int unitId, bool status, string? notes = null); // Cập nhật trạng thái thiết bị quản lí theo đơn vị
        Task<bool> DeleteUnitAsync(int unitId); // Xóa thiết bị quản lí theo đơn vị
    }
}
