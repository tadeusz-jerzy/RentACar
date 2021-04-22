using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RentACar.Infrastructure;
using System;
using Xunit;
using RentACar.Core.Entities;
using RentACar.Core.Services;
using RentACar.Core.DTOs;
using RentACar.Core.Interfaces;
using System.Threading.Tasks;
using RentACar.Core.Exceptions;
using Xunit.Abstractions;

namespace RentACar.IntegrationTests
{
    public class BookingServiceTest : InMemoryDbTest
    {
        async Task CreateOneCarUsingCarservice()
        {
            var cs = new CarService(db);
            var ms = new CarMakeService(db);

            await ms.CreateCarMakeFromDtoAsync(new CarMakeDTO { Name = "POLSKI FIAT" });
            await ms.CreateCarModelFromDtoAsync(new CarModelDTO { Name = "126P", CarMakeId = 1 });
            await cs.CreateCarFromDto(new CarCreateDTO
            {
                ModelId = 1,
                AcrissCode = "ABCD",
                RegistrationNumber = "00000",
                Vin = "00000000000000000",
                DailyPricePLN = 50
            });
        }
        public BookingServiceTest(ITestOutputHelper output) : base(output)
        {
            var userService = new UserService();
            bookingService = new BookingService(db, userService);
        }

        private readonly BookingService bookingService;

        private BookingDTO GetValidBookingDTO()
            => new BookingDTO
            {
                CarId = 1,
                Start = DateTime.Today.AddDays(1),
                End = DateTime.Today.AddDays(3),
                UserId = "JAN"
            };

#pragma warning disable IDE0051 // unused private members

        [Fact]
        async Task Test_ShouldCreateBookingFromValidDTO()

        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto = GetValidBookingDTO();

            // act
            var result = await bookingService.CreateBookingAsync(dto);

            // assert
            Assert.True(1 == result.Id); // we started with a clean DB
            Assert.Equal(dto.UserId, result.UserId);
            Assert.Equal(dto.CarId, result.CarId);
            Assert.Equal(dto.Start, result.Start);
            Assert.Equal(dto.End, result.End);

            // m acros.properties(RentACar.Core.DTOs.BookingDTO)
            //Assert.Equal(dto.${name}, result.${name});

        }


        [Fact]
        async Task Test_ShouldRejectBookingIfPeriodTooShort()
        {
            // this check is actually implemented by the Booking class
            // but tests should probably not care about that too much

            // arrange
            await CreateOneCarUsingCarservice();
            var dto = GetValidBookingDTO();
            dto.End = dto.Start.AddHours(1);

            // "act"
            Func<Task> act = async () => await bookingService.CreateBookingAsync(dto);

            // assert
            act.Should().ThrowExactly<InvalidDomainValueException>();

        }


        [Fact]
        async Task Test_ShouldAcceptBookingStartingToday()
        {
            // this check is actually implemented by the Booking class
            // but tests should probably not care about that too much

            // arrange
            await CreateOneCarUsingCarservice();
            var dto = GetValidBookingDTO();
            dto.Start = DateTime.Today;

            // act
            var result = await bookingService.CreateBookingAsync(dto);

            // assert
            Assert.True(0 < result.Id);
        }

        [Fact]
        async Task Test_ShouldRejectBookingThatOverlapsAnothersEnd()
        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto1 = GetValidBookingDTO();
            await bookingService.CreateBookingAsync(dto1);

            var dto2 = GetValidBookingDTO();

            // new booking starts before another ends
            dto2.Start = dto1.End.AddHours(-1);
            dto2.End = dto2.Start.AddDays(10);

            // "act"
            Func<Task> act = async () => await bookingService.CreateBookingAsync(dto2);

            // assert
            act.Should().ThrowExactly<ConflictingEntityException>();

        }

        [Fact]
        async Task Test_ShouldRejectBookingThatOverlapsAnothersStart()
        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto1 = GetValidBookingDTO();
            await bookingService.CreateBookingAsync(dto1);

            var dto2 = GetValidBookingDTO();

            // new booking ends after another starts
            dto2.Start = dto1.Start.AddDays(-10);
            dto2.End = dto1.Start.AddHours(1);

            // "act"
            Func<Task> act = async () => await bookingService.CreateBookingAsync(dto2);

            // assert
            act.Should().ThrowExactly<ConflictingEntityException>();
            
        }


        [Fact]
        async Task Test_ShouldRejectBookingThatStartsTooEarlyAfterAnother()
        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto1 = GetValidBookingDTO();
            await bookingService.CreateBookingAsync(dto1);

            var dto2 = GetValidBookingDTO();

            // new booking starts after another ends, but interval is too small
            dto2.Start = dto1.End.AddHours(0.9 * (double)Booking.HOURS_INTERVAL_BETWEEN_BOOKINGS);
            // new booking ends significantly later
            dto2.End = dto2.Start.AddDays(10);

            // "act"
            Func<Task> act = async () => await bookingService.CreateBookingAsync(dto2);

            // assert
            act.Should().ThrowExactly<ConflictingEntityException>();

        }

        [Fact]
        async Task Test_ShouldRejectBookingThatEndsTooLateBeforeAnother()
        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto1 = GetValidBookingDTO();
            await bookingService.CreateBookingAsync(dto1);

            var dto2 = GetValidBookingDTO();

            // new booking ends before another starts, but interval is too small
            dto2.End = dto1.Start.AddHours(-0.9 * (double)Booking.HOURS_INTERVAL_BETWEEN_BOOKINGS);
            // new booking starts significantly earlier
            dto2.Start = dto2.End.AddDays(-10);

            // "act"
            Func<Task> act = async () => await bookingService.CreateBookingAsync(dto2);

            // assert
            act.Should().ThrowExactly<ConflictingEntityException>();

        }


        [Fact]
        async Task Test_ShouldAcceptBookingForSufficientlyDisjointPeriod()
        {
            // arrange
            await CreateOneCarUsingCarservice();
            var dto1 = GetValidBookingDTO();
            await bookingService.CreateBookingAsync(dto1);

            var dto2 = GetValidBookingDTO();

            dto2.Start = dto1.End.AddHours(Booking.HOURS_INTERVAL_BETWEEN_BOOKINGS + 1);
            dto2.End = dto2.Start.AddDays(1);

            // act
            var booking2 = await bookingService.CreateBookingAsync(dto2);

            // assert
            Assert.True(2 == booking2.Id); // we started with a clean DB
            // macro-generated:
            Assert.Equal(dto2.UserId, booking2.UserId);
            Assert.Equal(dto2.CarId, booking2.CarId);
            Assert.Equal(dto2.Start, booking2.Start);
            Assert.Equal(dto2.End, booking2.End);
        }


    }
}
#pragma warning restore IDE0051 // Remove unused private members