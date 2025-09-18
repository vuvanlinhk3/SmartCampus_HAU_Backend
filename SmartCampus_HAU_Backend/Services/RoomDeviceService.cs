using SmartCampus_HAU_Backend.Exceptions.CustomExceptions;
using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.RoomDevices;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Mapper;
using SmartCampus_HAU_Backend.Data;
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

        public async Task<List<RoomDeviceDTO>> GetAllRoomDevicesAsync(int roomId)
        {
            var roomDevices = await _context.RoomDevices
                .Where(rd => rd.RoomId == roomId)
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

        public async Task<RoomDeviceDTO> AddRoomDeviceAsync(CreateRoomDeviceDTO createRoomDeviceDTO)
        {
            var room = await _context.Rooms.FindAsync(createRoomDeviceDTO.RoomId);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {createRoomDeviceDTO.RoomId} not found.");
            }
            var roomDevice = new RoomDevice
            {
                RoomId = createRoomDeviceDTO.RoomId,
                DeviceType = createRoomDeviceDTO.DeviceType,
                Quantity = createRoomDeviceDTO.Quantity,
                Status = createRoomDeviceDTO.Status,
                Detail = createRoomDeviceDTO.Detail
            };
            _context.RoomDevices.Add(roomDevice);
            await _context.SaveChangesAsync();
            return roomDevice.ToRoomDeviceDTO();
        }

        public async Task<RoomDeviceDTO> UpdateRoomDeviceAsync(int roomDeviceId, UpdateRoomDeviceDTO updateRoomDeviceDTO)
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

        public async Task<RoomDeviceDTO> UpdateRoomDeviceStatusAsync(int roomDeviceId, int quantity, bool newStatus, string? notes = null)
        {
            var roomDevice = await _context.RoomDevices.FindAsync(roomDeviceId);
            var roomDeviceChanged = await _context.RoomDevices
                .Where(rdc => rdc.RoomId == roomDevice!.RoomId && rdc.DeviceType == roomDevice.DeviceType && rdc.Status == false)
                .FirstOrDefaultAsync();
            var oldStatus = roomDevice!.Status;
            if (roomDevice == null)
            {
                throw new NotFoundException($"Room device with ID {roomDeviceId} not found.");
            }
            if (newStatus != oldStatus)
            {
                var history = new RoomDeviceStatusHistory
                {
                    RoomDeviceId = roomDeviceId,
                    DeviceType = roomDevice.DeviceType,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    ChangedAt = DateTime.UtcNow,
                    RoomId = roomDevice.RoomId,
                    QuantityAffected = quantity,
                    Notes = notes
                };
                _context.RoomDeviceStatusHistories.Add(history);

                if (newStatus == false)
                {
                    if (roomDeviceChanged != null)
                    {
                        roomDeviceChanged.Quantity += quantity;
                        roomDevice.Quantity -= quantity;
                        await _context.SaveChangesAsync();
                        return roomDevice.ToRoomDeviceDTO();
                    }
                    else
                    {
                        roomDeviceChanged = new RoomDevice
                        {
                            RoomId = roomDevice.RoomId,
                            DeviceType = roomDevice.DeviceType,
                            Quantity = quantity,
                            Status = newStatus,
                            Detail = roomDevice.Detail
                        };
                        _context.RoomDevices.Add(roomDeviceChanged);
                    }
                }
                else
                {
                    if (roomDeviceChanged != null)
                    {
                        roomDeviceChanged.Quantity -= quantity;
                        roomDevice.Quantity += quantity;
                        await _context.SaveChangesAsync();
                        return roomDevice.ToRoomDeviceDTO();
                    }
                    else
                    {
                        throw new BadRequestException($"No available broken device to fix for Room ID {roomDevice.RoomId} and Device Type {roomDevice.DeviceType}.");
                    }
                }
            }

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
