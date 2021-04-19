using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.API.Controllers;
using RentACar.Core.DTOs;
using RentACar.Core.Interfaces;
using RentACar.Core.QueryFilters;
using RentACar.Tests.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RentACar.UnitTests
{

    // controller testing may make most sense together with the framework 
    // ( https://andrewlock.net/should-you-unit-test-controllers-in-aspnetcore/ )
    // and we could then test i.e. DTO validation logic

    public class CarsControllerTests : TestWithOutput
    {
        public CarsControllerTests(ITestOutputHelper output) : base(output ) { }

        [Fact]
        public async Task GetCars_ReturnsNotFound_IfNoCarsFromService()
        {
            // Arrange
            var mockService = new Mock<ICarService>();
            mockService.Setup(svc => svc.GetCarsAsync(It.IsAny<CarQueryFilter>()))
                .ReturnsAsync(new List<CarForListingDTO>());

            TestOutput.WriteLine("hello");
            var controller = new CarsController(mockService.Object);

            // Act
            var result = await controller.GetCars(null);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(999)]
        public async Task GetCars_ReturnsOK_WithSameNumberOfCars_AsProvidedByService(int numCars)
        {
            // Arrange
            var mockService = new Mock<ICarService>();
            var listOfCarDTOs = new List<CarForListingDTO>();
            for (int i = 1; i <= numCars; i++)
                listOfCarDTOs.Add(new CarForListingDTO());
            
            mockService.Setup(svc => svc.GetCarsAsync(It.IsAny<CarQueryFilter>()))
                .ReturnsAsync(listOfCarDTOs);

            var controller = new CarsController(mockService.Object);

            // Act
            var result = await controller.GetCars(null);
            
            // Verify types and value
            var ok = (OkObjectResult)result.Result; 
            var list = (List<CarForListingDTO>)ok.Value;
            Assert.Equal(numCars, list.Count);

        }
    }
}
