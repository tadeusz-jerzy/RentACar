using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDTO>> GetUserBookingsAsync(string userId);

        Task<BookingDTO> GetActiveBookingAsync(int id);

        Task<BookingDTO> CreateBookingAsync (BookingDTO dto);

        Task<bool> CancelBookingAsync(int id);

    }
}
