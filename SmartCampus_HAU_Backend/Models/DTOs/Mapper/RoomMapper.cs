using SmartCampus_HAU_Backend.Models.DTOs.Rooms;
using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Models.DTOs.Mapper
{
    public static class RoomMapper
    {
        public static RoomDetailDTO ToRoomDetailDTO(this Room room)
        {
            return new RoomDetailDTO
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Location = room.Location,
                RoomType = room.RoomType
            };
        }

        public static RoomDetailDTO ToCreateRoomDTO(this Room room)
        {
            return new RoomDetailDTO
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Location = room.Location,
                RoomType = room.RoomType
            };
        }
    }
}
