using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.Exceptions;
using RentACar.Core.Interfaces;
using RentACar.Core.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RentACar.Core.Enumerations;
using System.Linq;

namespace RentACar.Core.Services
{
    public class CarSpecificationService : ICarSpecificationService 
    {

        private readonly IUnitOfWork _db;
        public CarSpecificationService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<CarModel> CreateCarFromDto(CarCreateDTO dto)
        {
            Car sameRegistration = await _db.Cars.GetByRegistrationAsync(dto.RegistrationNumber);

            if (sameRegistration != null)
                throw new ConflictingEntityException(
                    "car with this registration number already exists");

            Car sameVin = await _db.Cars.GetByVinAsync(dto.Vin);

            if (sameVin != null)
                throw new ConflictingEntityException(
                    "car with this VIN number already exists");


            // internal validation within Car class
            Car car = Car.FromDto(dto); // contains single entity-level validation

            // now, adding the car will not cause invalid state of the system
            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
            return MyMapper.Map(car);
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            // car must exist
            Car car = await _db.Cars.FindByIdAsync(id);

            if (car == null || car.Status == RentalCarStatus.Removed)
                throw new InvalidDomainValueException("car not found");

            // business rule: a deleted car must not have active bookings
            List<Booking> activeBookings = await _db.Bookings.GetManyAsync(
                b => b.CarId == id,
                b => b.Status == BookingStatus.Active,
                b => b.End > DateTime.Now
            );

            if (activeBookings.Count > 0)
                throw new ConflictingEntityException("Car with active bookings cannot be deleted");

            // going with default EF behavior for child entities
            car.Status = RentalCarStatus.Removed;
            _db.Cars.Update(car);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<CarForListingDTO> GetCarAsync(int id)
        {
            var car = await _db.Cars.FindByIdAsync(id);
            return MyMapper.Map(car);
        }


        public async Task<List<CarForListingDTO>> GetCarsAsync(CarQueryFilter filters)
        {
            // per defined repository interface
            List<Expression<Func<Car, bool>>> conditions = new List<Expression<Func<Car, bool>>>();

            // without deleted cars
            conditions.Add(c => c.Status == RentalCarStatus.Active);

            // no booking can overlap with requested period
            bool filteringByAvailability =
                    (filters.RequestedRentalStart != default)
                || (filters.RequestedRentalEnd != default);

            if (filteringByAvailability)
            {
                conditions.Add(c =>
                   !c.Bookings.Any(
                       b => (b.Start <= filters.RequestedRentalEnd && b.End >= filters.RequestedRentalStart)
                ));
            }


            // following code was initially macro-generated (CsharpMacros extension)
            // macrosss.properties(RentACar.Core.QueryFilters.CarQueryFilter)
            //if (filters.${name} != default)
            //    conditions.Add(c => c.${name} == filters.${name});

            if (filters.Make != default)
                conditions.Add(c => c.Specification.Make == filters.Make);

            if (filters.Model != default)
                conditions.Add(c => c.Specification.Model == filters.Model);

            if (filters.AcrissCode != default)
                conditions.Add(c => c.Specification.AcrissCode == filters.AcrissCode);

            if (filters.MinDailyPricePLN != default)
                conditions.Add(c => c.DailyPricePLN >= filters.MinDailyPricePLN);

            if (filters.MaxDailyPricePLN != default)
                conditions.Add(c => c.DailyPricePLN <= filters.MaxDailyPricePLN);


            List<Car> cars = await _db.Cars.GetManyAsync(conditions);

            List<CarForListingDTO> dtos = cars.Select(c => MyMapper.Map(c)).ToList();

            return dtos;

        }


    }
}
