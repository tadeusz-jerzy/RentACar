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
        private ICarService _carService;
        private IBookingService _bookingService;

        public Seed(ICarService carService, IBookingService bookingService)
        {
            _carService = carService;
            _bookingService = bookingService;
        }

        public void Create()
        {
            var cars = new List<CarCreateDTO>() {
            new CarCreateDTO(){
                Make = "POLSKI FIAT", Model = "126P", DailyPricePLN = 50,
                RegistrationNumber = "00001", Vin="00000000000000001", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                Make = "POLSKI FIAT", Model = "125P", DailyPricePLN = 60,
                RegistrationNumber = "00002", Vin="00000000000000002", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                Make = "FSO", Model = "WARSZAWA", DailyPricePLN = 70,
                RegistrationNumber = "00003", Vin="00000000000000003", AcrissCode = "ABCD" },
            new CarCreateDTO(){
                Make = "FSO", Model = "POLONEZ", DailyPricePLN = 100,
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
                    Start = new DateTime(2021,5,5), End = new DateTime(2021,5,8)
                }
            };
            bookings.ForEach(async b => await _bookingService.CreateBookingAsync(b));
            
        }
    }


}
