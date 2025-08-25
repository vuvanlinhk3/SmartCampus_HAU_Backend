using SmartCampus_HAU_Backend.Models.DTOs.RoomDevices;
using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Models.DTOs.Mapper
{
    public static class RoomDeviceMapper
    {
        public static RoomDeviceDTO ToRoomDeviceDTO(this RoomDevice roomDevice)
        {
            return new RoomDeviceDTO
            {
                RoomDeviceId = roomDevice.RoomDeviceId,
                RoomId = roomDevice.RoomId,
                DeviceType = roomDevice.DeviceType,
                Quantity = roomDevice.Quantity,
                Status = roomDevice.Status,
                Detail = roomDevice.Detail
            };
        }
    }
}
