using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Models.DTOs.Units;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUnitService
    {
        Task<List<UnitDTO>> GetAllUnitsAsync(); // Lấy tất cả thiết bị quản lí theo đơn vị trong phòng
        Task<UnitDTO> CreateUnitAsync(UnitDTO createUnitDTO); // Tạo mới thiết bị quản lí theo đơn vị
        Task<UnitDTO> GetUnitByIdAsync(int unitId); // Lấy thiết bị quản lí theo đơn vị theo ID
        Task<UnitDTO> UpdateUnitAsync(int unitId, UnitDTO unitDTO); // Cập nhật thiết bị quản lí theo đơn vị
        Task<IActionResult> DeleteUnitAsync(int unitId); // Xóa thiết bị quản lí theo đơn vị
    }
}
