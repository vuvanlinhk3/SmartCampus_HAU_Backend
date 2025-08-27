using SmartCampus_HAU_Backend.Models.DTOs.Units;
using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Models.DTOs.Mapper
{
    public static class UnitMapper
    {
        public static UnitDTO ToUnitDTO(this Unit unit)
        {
            return new UnitDTO
            {
                UnitId = unit.UnitId,
                RoomId = unit.RoomId,
                DeviceType = unit.DeviceType,
                DeviceCode = unit.DeviceCode,
                Status = unit.Status,
                Detail = unit.Detail
            };
        }
    }
}
