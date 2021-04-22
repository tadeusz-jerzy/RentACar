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
    public class CarService : ICarService // within "Car" aggregate root
    {

        private readonly IUnitOfWork _db;
        public CarService(IUnitOfWork db)
        {
            _db = db;
        }

        
        public async Task<CarForListingDTO> CreateCarFromDto(CarCreateDTO dto)
        {
            bool sameRegistrationExists = await _db.Cars.CarWithRegistrationExists(dto.RegistrationNumber);

            if (sameRegistrationExists)
                throw new ConflictingEntityException(
                    "car with this registration number already exists");

            bool sameVinExists = await _db.Cars.CarWithVinExists(dto.Vin);
            if (sameVinExists)
                throw new ConflictingEntityException(
                    "car with this VIN number already exists");


            CarModel carModel = await _db.CarModels.FindByIdAsync(dto.ModelId);
            if (carModel == null)
                throw new EntityNotFoundException("car model not found for given car model id");

            // checks domain-level rules internally
            var spec = CarSpecification.FromModelAndAcriss(carModel, dto.AcrissCode);

            var vin = new Vin(dto.Vin);

            Car car = new Car(spec, vin, dto.RegistrationNumber, dto.DailyPricePLN);
            
            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
            // reload with make, model
            var withMakeModel = await _db.Cars.FindByIdAsync(car.Id);
            CarForListingDTO forListing = MyMapper.Map(withMakeModel);
            return forListing ;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            // car must exist
            Car car = await _db.Cars.FindByIdAsync(id);

            if (car == null || car.Status == RentalCarStatus.Removed)
                throw new EntityNotFoundException("car not found");

            // business rule: a deleted car must not have active bookings
            List < Booking> activeBookings = await _db.Bookings.GetManyAsync(
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
                ||  (filters.RequestedRentalEnd != default);

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
                conditions.Add(c => c.Specification.Make.Name == filters.Make);
            
            if (filters.Model != default)
                conditions.Add(c => c.Specification.Model.Name == filters.Model);
            
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
