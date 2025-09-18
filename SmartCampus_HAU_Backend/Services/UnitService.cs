using SmartCampus_HAU_Backend.Exceptions.CustomExceptions;
using SmartCampus_HAU_Backend.Models.DTOs.Units;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Mapper;
using SmartCampus_HAU_Backend.Data;
using Microsoft.EntityFrameworkCore;
using SmartCampus_HAU_Backend.Services.Interfaces;

namespace SmartCampus_HAU_Backend.Services
{
    public class UnitService : IUnitService
    {
        private readonly ApplicationDbContext _context;
        public UnitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnitDTO>> GetAllUnitsAsync(int roomId)
        {
            var units = await _context.Units
                .Where(u => u.RoomId == roomId)
                .ToListAsync();

            return units.Select(u => u.ToUnitDTO()).ToList();
        }

        public async Task<UnitDTO> GetUnitByIdAsync(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
            {
                throw new NotFoundException($"Unit with ID {unitId} not found.");
            }
            return unit.ToUnitDTO();
        }

        public async Task<UnitDTO> AddUnitAsync(CreateUnitDTO createUnitDTO)
        {
            var unit = new Unit
            {
                RoomId = createUnitDTO.RoomId,
                DeviceType = createUnitDTO.DeviceType,
                DeviceCode = createUnitDTO.DeviceCode,
                Status = createUnitDTO.Status,
                Detail = createUnitDTO.Detail
            };
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            return unit.ToUnitDTO();
        }

        public async Task<UnitDTO> UpdateUnitAsync(int unitId, UpdateUnitDTO updateUnitDTO)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
            {
                throw new NotFoundException($"Unit with ID {unitId} not found.");
            }

            unit.DeviceType = updateUnitDTO.DeviceType;
            unit.DeviceCode = updateUnitDTO.DeviceCode;
            unit.Status = updateUnitDTO.Status;
            unit.Detail = updateUnitDTO.Detail;
            await _context.SaveChangesAsync();
            return unit.ToUnitDTO();
        }

        public async Task<UnitDTO> UpdateUnitStatusAsync(int unitId, bool newStatus, string? notes = null)
        {
            var unit = await _context.Units.FindAsync(unitId);
            var oldStatus = unit!.Status;
            if (unit == null)
            {
                throw new NotFoundException($"Unit with ID {unitId} not found.");
            }
            if (oldStatus != newStatus)
            {
                var history = new UnitStatusHistory
                {
                    UnitId = unitId,
                    DeviceType = unit.DeviceType,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    ChangedAt = DateTime.UtcNow,
                    RoomId = unit.RoomId,
                    Notes = notes
                };

                _context.UnitStatusHistories.Add(history);
                unit.Status = newStatus;
                await _context.SaveChangesAsync();
            }
            return unit.ToUnitDTO();
        }

        public async Task<bool> DeleteUnitAsync(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
            {
                throw new NotFoundException($"Unit with ID {unitId} not found.");
            }
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
