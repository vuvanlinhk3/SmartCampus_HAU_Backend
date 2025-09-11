using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Exceptions.CustomExceptions;
using SmartCampus_HAU_Backend.Models.DTOs.Bookings;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Mapper;
using SmartCampus_HAU_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingDTO>> GetAllBookingsAsync(int roomId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.RoomId == roomId)
                .ToListAsync();
            return bookings.Select(b => b.ToBookingDTO()).ToList();
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }
            return booking.ToBookingDTO();
        }

        public async Task<BookingDTO> AddBookingAsync(CreateBookingDTO createBookingDTO)
        {
            if (createBookingDTO.StartPeriod < 1 || createBookingDTO.StartPeriod > 18)
                throw new BadRequestException("StartPeriod phải từ 1-18");

            if (createBookingDTO.Periods < 1 || createBookingDTO.StartPeriod + createBookingDTO.Periods - 1 > 18)
                throw new BadRequestException("Periods không hợp lệ hoặc vượt quá tiết 18");

            var booking = new Booking
            {
                RoomId = createBookingDTO.RoomId,
                ClassName = createBookingDTO.ClassName,
                Subject = createBookingDTO.Subject,
                Teacher = createBookingDTO.Teacher,
                RegisteredBy = createBookingDTO.RegisteredBy,
                BookingDate = createBookingDTO.BookingDate.ToLocalTime().Date,
                StartPeriod = createBookingDTO.StartPeriod,
                Periods = createBookingDTO.Periods,
                CreatedAt = DateTime.Now
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.ToBookingDTO();
        }

        public async Task<BookingDTO> UpdateBookingAsync(int bookingId, BookingDTO bookingDTO)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }
            booking.ClassName = bookingDTO.ClassName;
            booking.Subject = bookingDTO.Subject;
            booking.Teacher = bookingDTO.Teacher;
            booking.RegisteredBy = bookingDTO.RegisteredBy;
            booking.BookingDate = bookingDTO.BookingDate;
            booking.StartPeriod = bookingDTO.StartPeriod;
            booking.Periods = bookingDTO.Periods;
            await _context.SaveChangesAsync();
            return booking.ToBookingDTO();
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
