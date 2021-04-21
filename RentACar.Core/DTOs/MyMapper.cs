using RentACar.Core.Entities;

// includes generated code (mapper extension by Cezary Piątek)
// https://cezarypiatek.github.io/post/why-i-dont-use-automapper/
// mappings live close to DTOs and should change with them

// domain entities must not exist in invalid state,
// therefore no automatic mapping from dtos to entities is used
// (business rules must be enforced when creating entities)


namespace RentACar.Core.DTOs
{
    public static class MyMapper
    {


        public static CarForListingDTO Map(Car car)
        {
            return new CarForListingDTO
            {
                Id = car.Id,
                DailyPricePLN = car.DailyPricePLN,
                Make = car.Specification.Make.Name,
                Model = car.Specification.Model.Name,
                AcrissCode = car.Specification.AcrissCode
            };
        }


        public static BookingDTO Map(Booking b)
        {
            return new BookingDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                CarId = b.CarId,
                Start = b.Start,
                End = b.End
            };
        }

        public static CarMakeDTO Map(CarMake carMake)
        {
            return new CarMakeDTO
            {
                Id = carMake.Id,
                Name = carMake.Name
            };
        }

        public static CarModelDTO Map(CarModel carModel)
        {
            return new CarModelDTO
            {
                Id = carModel.Id,
                CarMakeId = carModel.CarMakeId,
                Name = carModel.Name
            };
        }
    }
}
