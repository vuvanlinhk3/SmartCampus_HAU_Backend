using SmartCampus_HAU_Backend.Exceptions.CustomExceptions;
using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Rooms;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Mapper;
using SmartCampus_HAU_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly Dictionary<int, (TimeSpan Start, TimeSpan End)> _periodTimes = new()
        {
            { 1, (new TimeSpan(6, 55, 0), new TimeSpan(7, 40, 0)) },
            { 2, (new TimeSpan(7, 45, 0), new TimeSpan(8, 30, 0)) },
            { 3, (new TimeSpan(8, 35, 0), new TimeSpan(9, 20, 0)) },
            { 4, (new TimeSpan(9, 30, 0), new TimeSpan(10, 15, 0)) },
            { 5, (new TimeSpan(10, 20, 0), new TimeSpan(11, 5, 0)) },
            { 6, (new TimeSpan(11, 10, 0), new TimeSpan(11, 55, 0)) },
            { 7, (new TimeSpan(12, 5, 0), new TimeSpan(12, 50, 0)) },
            { 8, (new TimeSpan(12, 55, 0), new TimeSpan(13, 40, 0)) },
            { 9, (new TimeSpan(13, 45, 0), new TimeSpan(14, 30, 0)) },
            { 10, (new TimeSpan(14, 40, 0), new TimeSpan(15, 25, 0)) },
            { 11, (new TimeSpan(15, 30, 0), new TimeSpan(16, 15, 0)) },
            { 12, (new TimeSpan(16, 20, 0), new TimeSpan(17, 5, 0)) }
        };

        public async Task<List<AllRoomWithStatusDTO>> GetAllRoomsWithStatus()
        {
            try
            {
                var currentDate = DateTime.Today;
                var currentTime = DateTime.Now.TimeOfDay;

                // Lấy tất cả phòng và booking trong ngày hiện tại
                var roomsWithBookings = await (from room in _context.Rooms
                                               from booking in _context.Bookings
                                                   .Where(b => b.RoomId == room.RoomId &&
                                                              b.BookingDate.Date == currentDate)
                                                   .DefaultIfEmpty()
                                               select new
                                               {
                                                   room.RoomId,
                                                   room.RoomName,
                                                   BookingId = booking != null ? (int?)booking.BookingId : null,
                                                   Subject = booking != null ? booking.Subject : null,
                                                   Teacher = booking != null ? booking.Teacher : null,
                                                   StartPeriod = booking != null ? booking.StartPeriod : (int?)null,
                                                   Periods = booking != null ? booking.Periods : (int?)null,
                                                   BookingDate = booking != null ? booking.BookingDate : (DateTime?)null
                                               })
                    .ToListAsync();

                // Nhóm theo phòng và xử lý logic trạng thái
                var rooms = roomsWithBookings
                    .GroupBy(x => new { x.RoomId, x.RoomName })
                    .Select(g =>
                    {
                        // Tìm booking đang diễn ra hiện tại
                        var activeBooking = g.FirstOrDefault(x =>
                            x.StartPeriod.HasValue && x.Periods.HasValue &&
                            IsBookingActiveNow(x.StartPeriod.Value, x.Periods.Value, currentTime));

                        // Nếu không có booking đang diễn ra, lấy booking gần nhất trong ngày
                        var nearestBooking = g
                            .Where(x => x.BookingId.HasValue && x.StartPeriod.HasValue)
                            .OrderBy(x => x.StartPeriod)
                            .FirstOrDefault();

                        // Quyết định booking nào sẽ được hiển thị
                        var displayBooking = activeBooking ?? nearestBooking;

                        return new AllRoomWithStatusDTO
                        {
                            RoomId = g.Key.RoomId,
                            RoomName = g.Key.RoomName,
                            Status = activeBooking != null ? "Đang học" : "Trống",
                            Subject = displayBooking?.Subject,
                            Teacher = displayBooking?.Teacher,
                            TimeRange = GetTimeRange(displayBooking?.StartPeriod, displayBooking?.Periods),
                            BookingDate = displayBooking?.BookingDate ?? DateTime.Today
                        };
                    })
                    .OrderBy(r => r.RoomName)
                    .ToList();

                return rooms;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi hệ thống: {ex.Message}", ex);
            }
        }

        // Kiểm tra booking có đang diễn ra không
        private bool IsBookingActiveNow(int startPeriod, int periods, TimeSpan currentTime)
        {
            if (!_periodTimes.ContainsKey(startPeriod))
                return false;

            var bookingStartTime = _periodTimes[startPeriod].Start;
            var endPeriod = startPeriod + periods - 1;

            if (!_periodTimes.ContainsKey(endPeriod))
                return false;

            var bookingEndTime = _periodTimes[endPeriod].End;

            return currentTime >= bookingStartTime && currentTime <= bookingEndTime;
        }

        // Chuyển đổi StartPeriod và Periods thành TimeRange
        private string? GetTimeRange(int? startPeriod, int? periods)
        {
            if (!startPeriod.HasValue || !periods.HasValue ||
                startPeriod <= 0 || periods <= 0)
                return null;

            if (!_periodTimes.ContainsKey(startPeriod.Value))
                return null;

            var startTime = _periodTimes[startPeriod.Value].Start;
            var endPeriod = startPeriod.Value + periods.Value - 1;

            if (!_periodTimes.ContainsKey(endPeriod))
                return null;

            var endTime = _periodTimes[endPeriod].End;

            return $"{startTime:hh\\:mm} - {endTime:hh\\:mm}";
        }

        public async Task<CreateRoomDTO> AddRoomAsync(CreateRoomDTO createRoomDTO)
        {
            if (createRoomDTO == null || string.IsNullOrEmpty(createRoomDTO.RoomName))
            {
                throw new BadRequestException("Invalid room data.");
            }
            var room = new Room
            {
                RoomName = createRoomDTO.RoomName,
                Location = createRoomDTO.Location,
                RoomType = createRoomDTO.RoomType,
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room.ToCreateRoomDTO();
        }

        public async Task<RoomDetailDTO> GetRoomByNameAsync(string roomName)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.RoomName.Contains(roomName))
                .ToListAsync();

            if (!room.Any())
            {
                throw new NotFoundException($"Room with name '{roomName}' not found.");
            }

            return room.First().ToRoomDetailDTO();
        }

        public async Task<RoomDetailDTO> UpdateRoomAsync(int roomId, RoomDetailDTO roomDetailDTO)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {roomId} not found.");
            }

            room.RoomName = roomDetailDTO.RoomName ?? room.RoomName;
            room.Location = roomDetailDTO.Location;
            room.RoomType = roomDetailDTO.RoomType ?? room.RoomType;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return room.ToRoomDetailDTO();
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {roomId} not found.");
            }
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
