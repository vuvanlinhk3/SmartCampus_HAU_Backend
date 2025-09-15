using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("booking/getall/{roomId}")]
        public async Task<IActionResult> GetAllBookings([FromRoute] int roomId)
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync(roomId);
                return new OkObjectResult(bookings);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("booking/getallforstatistic")]
        public async Task<IActionResult> GetAllBookingsForStatistic()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsForStatisticAsync();
                return new OkObjectResult(bookings);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("booking/getbyid/{bookingId}")]
        public async Task<IActionResult> GetBookingById( [FromRoute] int bookingId)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(bookingId);
                return new OkObjectResult(booking);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("booking/add")]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
            if (createBookingDTO == null)
            {
                return new BadRequestObjectResult("Invalid booking data.");
            }
            try
            {
                var result = await _bookingService.AddBookingAsync(createBookingDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("booking/update/{bookingId}")]
        public async Task<IActionResult> UpdateBooking([FromRoute] int bookingId, [FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO == null)
            {
                return new BadRequestObjectResult("Invalid booking data.");
            }
            try
            {
                var result = await _bookingService.UpdateBookingAsync(bookingId, bookingDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("booking/delete/{bookingId}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] int bookingId)
        {
            try
            {
                await _bookingService.DeleteBookingAsync(bookingId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
