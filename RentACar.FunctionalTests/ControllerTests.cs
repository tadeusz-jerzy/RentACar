using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RentACar.Core.DTOs;
using RentACar.API.Controllers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RentACar.FunctionalTests
{
    public class CarsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }
        private readonly ITestOutputHelper _output;

        public CarsControllerTests(WebApplicationFactory<Startup> fixture, ITestOutputHelper output)
        {
            Client = fixture.CreateClient();
            _output = output;
        }

        [Theory]
        [InlineData("/cars")]
        [InlineData("/cars/1")]
        [InlineData("/cars/10")]
        [InlineData("/bookings/1")]
        [InlineData("/cars?make=FSO")]

        // cache setting from Startup is visible to the unit test
        // but is subsequently overridden via controller method attribute

        // so, and external client gets a different header
        // so apparently if we use the attributes, we must test from further outside the app
        // than the WebApplicationFactory.CreateClient()
        
        // (for example Postman GUI + Newman CLI )
        public async Task Get_AllowsCaching(string endpoint)
        {
            var response = await Client.GetAsync(endpoint);
            var headers = response.Headers;

            Assert.NotEmpty(response.Headers);
            
            Assert.NotNull(response.Headers.CacheControl);
            Assert.Equal(Startup.CACHE_MAX_AGE_SECONDS, response.Headers.CacheControl.MaxAge.Value.TotalSeconds );
            // fails:
            // Assert.Equal(CarsController.CACHE_MAX_AGE_SECONDS, response.Headers.CacheControl.MaxAge.Value.TotalSeconds);

            _output.WriteLine(headers.CacheControl.MaxAge.ToString());
        }


        [Fact]
        public async Task CarsPost_ReturnsCreatedWithNoCacheHeaderForAValidDTO()
        {
            
            // arrange
            var dto = new CarCreateDTO
            {
                Make = "POLSKI FIAT",
                Model = "126P",
                AcrissCode = "ABCD",
                RegistrationNumber = "00000",
                Vin = "00000000000000000",
                DailyPricePLN = 50
            };
            
            var json = JsonConvert.SerializeObject(dto);
            
            HttpContent request = new StringContent(json, Encoding.UTF8, "application/json");
            
            // act
            var response = await Client.PostAsync("/api/cars", request);

            // assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.NotEmpty(response.Headers);
            Assert.NotNull(response.Headers.CacheControl);
            Assert.True(response.Headers.CacheControl.NoCache);
            Assert.True(response.Headers.CacheControl.NoCache);
 
        }

        [Fact]
        public async Task CarsPost_ReturnsBadRequestAndErrorsListForDTOValidationErrors()
        {

            // arrange
            var dto = new CarCreateDTO
            {
                Make = "HONDA", // DTO format OK
                Model = "CIVIC", // DTO format OK
                AcrissCode = "ABCDEFG", // too long
                RegistrationNumber = "00", // too short
                Vin = null, // missing, required
                DailyPricePLN = 50 // DTO format OK
            };
            var requestJson = JsonConvert.SerializeObject(dto);
            HttpContent request = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // act
            HttpResponseMessage response = await Client.PostAsync("/api/cars", request);
            string responseJson = await response.Content.ReadAsStringAsync();
            // https://makolyte.com/csharp-deserialize-json-to-dynamic-object/
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(responseJson, new ExpandoObjectConverter());
            var errors = (IDictionary<string, Object>)result.errors; // https://stackoverflow.com/a/23753103
            
            // assert
            Assert.True(errors.ContainsKey("RegistrationNumber"));
            Assert.True(errors.ContainsKey("AcrissCode"));
            Assert.True(errors.ContainsKey("Vin"));
            Assert.Equal(3, errors.Count); // no more errors
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
