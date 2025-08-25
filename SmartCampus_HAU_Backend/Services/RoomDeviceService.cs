using SmartCampus_HAU_Backend.Exceptions.CustomExceptions;
using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.RoomDevices;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Mapper;
using SmartCampus_HAU_Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class RoomDeviceService : IRoomDeviceService
    {
        private readonly ApplicationDbContext _context;

        public RoomDeviceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDeviceDTO>> GetAllRoomDevicesAsync(int roomID)
        {
            var roomDevices = await _context.RoomDevices
                .Where(rd => rd.RoomId == roomID)
                .ToListAsync();

            return roomDevices.Select(rd => rd.ToRoomDeviceDTO()).ToList();
        }

        public async Task<RoomDeviceDTO> GetRoomDeviceByIdAsync(int roomDeviceId)
        {
            var roomDevice = await _context.RoomDevices.FindAsync(roomDeviceId);
            if (roomDevice == null)
            {
                throw new NotFoundException($"Room device with ID {roomDeviceId} not found.");
            }
            return roomDevice.ToRoomDeviceDTO();
        }

        public async Task<RoomDeviceDTO> CreateRoomDeviceAsync(int roomId, RoomDeviceDTO createRoomDeviceDTO)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {roomId} not found.");
            }
            var roomDevice = new RoomDevice
            {
                RoomId = roomId,
                DeviceType = createRoomDeviceDTO.DeviceType,
                Quantity = createRoomDeviceDTO.Quantity,
                Status = createRoomDeviceDTO.Status,
                Detail = createRoomDeviceDTO.Detail
            };
            _context.RoomDevices.Add(roomDevice);
            await _context.SaveChangesAsync();
            return roomDevice.ToRoomDeviceDTO();
        }

        public async Task<RoomDeviceDTO> UpdateRoomDeviceAsync(int roomDeviceId, RoomDeviceDTO updateRoomDeviceDTO)
        {
            var roomDevice = await _context.RoomDevices.FindAsync(roomDeviceId);
            if (roomDevice == null)
            {
                throw new NotFoundException($"Room device with ID {roomDeviceId} not found.");
            }
            roomDevice.DeviceType = updateRoomDeviceDTO.DeviceType;
            roomDevice.Quantity = updateRoomDeviceDTO.Quantity;
            roomDevice.Status = updateRoomDeviceDTO.Status;
            roomDevice.Detail = updateRoomDeviceDTO.Detail;
            _context.RoomDevices.Update(roomDevice);
            await _context.SaveChangesAsync();
            return roomDevice.ToRoomDeviceDTO();
        }

        public async Task<bool> DeleteRoomDeviceAsync(int roomDeviceId)
        {
            var roomDevice = await _context.RoomDevices.FindAsync(roomDeviceId);
            if (roomDevice == null)
            {
                throw new NotFoundException($"Room device with ID {roomDeviceId} not found.");
            }
            _context.RoomDevices.Remove(roomDevice);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
