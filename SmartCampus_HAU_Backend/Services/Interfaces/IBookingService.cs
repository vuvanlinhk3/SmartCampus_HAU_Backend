using SmartCampus_HAU_Backend.Models.DTOs.Bookings;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDTO>> GetAllBookingsAsync(int roomId); // Lấy tất cả đặt phòng theo phòng
        Task<BookingDTO> AddBookingAsync(int roomId, BookingDTO createBookingDTO); // Tạo mới đặt phòng
        Task<BookingDTO> GetBookingByIdAsync(int bookingId); // Lấy đặt phòng theo ID
        Task<BookingDTO> UpdateBookingAsync(int bookingId, BookingDTO bookingDTO); // Cập nhật đặt phòng
        Task<bool> DeleteBookingAsync(int bookingId); // Xóa đặt phòng
    }
}
