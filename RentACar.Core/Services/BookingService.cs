using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.Enumerations;
using RentACar.Core.Exceptions;
using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Core.Services
{
    /// <summary>
    /// allows making a booking, preserving a domain invariant within Car aggregate root, 
    /// ( one car must never have two overlapping bookings )
    /// also references user service
    /// </summary>
    public class BookingService : IBookingService 
    {

        private readonly IUnitOfWork _db;
        private readonly IUserService _user;

        public BookingService(IUnitOfWork db, IUserService user)
        {
            _db = db;
            _user = user;
        }


        public async Task<BookingDTO> CreateBookingAsync(BookingDTO dto)
        {

            // booking can only exist for an existing (active, not deleted) car
            Car car = await _db.Cars.FindByIdAsync(dto.CarId);
            if (car == null || car.Status != RentalCarStatus.Active)
                throw new BusinessException("car not found");

            // booking must have a valid user id
            if (!_user.UserExists(dto.UserId))
                throw new EntityNotFoundException(
                    "user id not found");

            // no two bookings can overlap for one car aggregate root
            DateTime conflictingPeriodStart = dto.Start.AddHours(-1 * Booking.HOURS_INTERVAL_BETWEEN_BOOKINGS);
            DateTime conflictingPeriodEnd = dto.End.AddHours(Booking.HOURS_INTERVAL_BETWEEN_BOOKINGS);
            Booking conflictingBooking = await _db.Bookings.GetOneAsync(
                b => b.CarId == car.Id,
                b => (b.Start < conflictingPeriodEnd && b.End >= conflictingPeriodStart)
            );

            if (conflictingBooking != null)
                throw new ConflictingEntityException(
                    "car already has a booking which overlaps requested period");

            // internal validation inside Booking class
            Booking booking = Booking.FromDto(dto);

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return MyMapper.Map(booking);
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            Booking booking = await _db.Bookings.FindByIdAsync(id);
            booking.Status = BookingStatus.Cancelled;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<BookingDTO> GetActiveBookingAsync(int id)
        {
            Booking booking = await _db.Bookings.FindByIdAsync(id);
            
            if (booking == default || booking.Status != BookingStatus.Active)
                return null;

            return MyMapper.Map(booking);
        }

        public async Task<List<BookingDTO>> GetUserBookingsAsync(string userId)
        {
            List<Booking> bookings = await _db.Bookings.GetManyAsync(
                b => b.UserId == userId);

            List<BookingDTO> dtos = bookings.Select(b => MyMapper.Map(b)).ToList();
            
            return dtos;
        }

    }
}
