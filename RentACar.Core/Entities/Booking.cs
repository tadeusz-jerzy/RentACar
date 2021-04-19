using RentACar.Core.DTOs;
using RentACar.Core.Enumerations;
using RentACar.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities
{
    public class Booking: BaseEntity
    {
        // business rule, for car washing etc
        public const int HOURS_INTERVAL_BETWEEN_BOOKINGS = 24;

        private Booking() { }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public Car RentalCar { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public BookingStatus Status { get; set; }

        // validation: end > start, car id points to a car

        public bool OverlapsWithPeriod(DateTime PeriodStart, DateTime PeriodEnd)
            => (Start < PeriodEnd) && (End > PeriodStart);

        internal static Booking FromDto(BookingDTO dto)
        {
            // business rules for fields not related to other entities
            if (dto.Start < DateTime.Today)
                throw new InvalidDomainValueException(
                    "booking start date must be in the future or today");

            if (dto.End < dto.Start.AddDays(1))
                throw new InvalidDomainValueException(
                    "booking end date must be at least 1 day later than start date");

            return new Booking()
            {
                UserId = dto.UserId,
                CarId = dto.CarId,
                Start = dto.Start,
                End = dto.End,
                Status = BookingStatus.Active
            };
        }
    }
}
