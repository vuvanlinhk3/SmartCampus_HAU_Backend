using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Models.DTOs.Bookings;

namespace SmartCampus_HAU_Backend.Models.DTOs.Mapper
{
    public static class BookingMapper
    {
        public static BookingDTO ToBookingDTO(this Booking booking)
        {
            if (booking == null) return null;
            return new BookingDTO
            {
                BookingId = booking.BookingId,
                RoomId = booking.RoomId,
                Subject = booking.Subject,
                Teacher = booking.Teacher,
                StartPeriod = booking.StartPeriod,
                Periods = booking.Periods,
                BookingDate = booking.BookingDate
            };
        }
    }
}
