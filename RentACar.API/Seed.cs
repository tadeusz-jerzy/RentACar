using RentACar.Core.DTOs;
using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.API
{
    public interface ISeed { void Create(); }

    public class Seed : ISeed
    {
        private readonly ICarService _carService;
        private readonly IBookingService _bookingService;
        private readonly ICarMakeService _carMakeService;

        public Seed(ICarService carService, 
            IBookingService bookingService,
            ICarMakeService carMakeService)
        {
            _carService = carService;
            _bookingService = bookingService;
            _carMakeService = carMakeService;
        }

        public void Create()
        {
            var makes = new List<CarMakeDTO>()
            {
                new CarMakeDTO() {Name = "POLSKI FIAT" },
                new CarMakeDTO() {Name = "FSO" }
            };

            makes.ForEach(async m => await _carMakeService.CreateCarMakeFromDtoAsync(m));

            var models = new List<CarModelDTO>()
            {
                new CarModelDTO() {Name = "126P", CarMakeId = 1 },
                new CarModelDTO() {Name = "125P", CarMakeId = 1 },
                new CarModelDTO() {Name = "POLONEZ", CarMakeId = 2 },
                new CarModelDTO() {Name = "WARSZAWA", CarMakeId = 2 },
            };

            models.ForEach(async m => await _carMakeService.CreateCarModelFromDtoAsync(m));

            var cars = new List<CarCreateDTO>() {
            new CarCreateDTO(){
                ModelId = 1, DailyPricePLN = 50,
                RegistrationNumber = "00001", Vin="00000000000000001", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                ModelId = 2, DailyPricePLN = 60,
                RegistrationNumber = "00002", Vin="00000000000000002", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                ModelId = 3, DailyPricePLN = 70,
                RegistrationNumber = "00003", Vin="00000000000000003", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                ModelId = 4, DailyPricePLN = 100,
                RegistrationNumber = "00004", Vin="00000000000000004", AcrissCode = "ABCD" },
            };

            cars.ForEach(async c => await _carService.CreateCarFromDto(c));

            var bookings = new List<BookingDTO>()
            {
                new BookingDTO()
                {
                    CarId = 1, UserId = "JAN KOWALSKI",
                    Start = new DateTime(2021,4,30), End = new DateTime(2021,5,4)
                },
                new BookingDTO()
                {
                    CarId = 1, UserId = "ALICJA NOWAK",
                    Start = new DateTime(2021,5,6), End = new DateTime(2021,5,8)
                }
            };
            bookings.ForEach(async b => await _bookingService.CreateBookingAsync(b));
            
        }
    }


}
