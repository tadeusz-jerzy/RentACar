using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET api/bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneBooking(int id)
        {
            var dto = await _bookingService.GetActiveBookingAsync(id);

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        // POST api/bookings
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookingDTO createDTO)
        {
            var newDto = await _bookingService.CreateBookingAsync(createDTO);
            return Created($"/api/bookings/{newDto.Id}", newDto);
        }

        // DELETE api/bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _bookingService.CancelBookingAsync(id);
            return Ok(true);
        }
    }
}
