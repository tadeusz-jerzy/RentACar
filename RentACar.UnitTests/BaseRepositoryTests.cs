using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Infrastructure;
using RentACar.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RentACar.UnitTests
{

    public class BaseRepositoryTests : InMemoryDbTest
    {
        // one proposed approach could be
        // https://rubikscode.net/2018/04/16/implementing-and-testing-repository-pattern-using-entity-framework/

        // however, MS advices strongly against mocking EF Core objects like DB contexts 
        // https://docs.microsoft.com/pl-pl/ef/core/testing/

        // so, for a meaningful test, I'll use the repository and then verify end state of EF context
        // although would be best to actually run against i.e. LocalDB
        // because InMemory behaves differently from actual db


        public BaseRepositoryTests(ITestOutputHelper output) : base(output)
        {
            UseFreshDb();
        }

        private Car GetValidCar() =>
            Car.FromDto(new CarCreateDTO
            {
                Make = "POLSKI FIAT",
                Model = "126P",
                AcrissCode = "ABCD",
                RegistrationNumber = "00000",
                Vin = "00000000000000000",
                DailyPricePLN = 50
            });

        [Fact]
        public void Add_CarAddedToDbContext()
        {

            // Arrange
            Car car = GetValidCar();
            var sut = new BaseRepository<Car>(context);

            // act
            sut.Add(car);

            // assert
            var entries = context.ChangeTracker.Entries<Car>().ToList();
            Assert.Single(entries);
            Assert.Equal(EntityState.Added, entries[0].State);

        }

        [Fact]
        public async Task GetAll_CarsCanBeReadFromDb()
        {

            // Arrange
            Car car = GetValidCar();
            Car car2 = GetValidCar();
            var repository = new BaseRepository<Car>(context);
            repository.Add(car);
            repository.Add(car2);
            context.SaveChanges(); // in-memory does not really need this... 

            // act
            var newContext = GetNewContext();
            
            var sut = new BaseRepository<Car>(newContext);
            var list = await sut.GetAllAsync();

            // assert
            Assert.Equal(2, list.Count);
            Assert.Equal(car.RegistrationNumber, list[0].RegistrationNumber);

        }


    }
}
