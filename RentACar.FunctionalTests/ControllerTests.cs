using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
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
        // the following setting is visible to the unit test
        // but is subsequently overridden via controller method attribute
        // and external client gets a different header
        // so apparently if we use the attributes, we must test from further outside the app
        // than the WebApplicationFactory.CreateClient()
        public async Task Get_AllowsCaching(string endpoint)
        {
            var response = await Client.GetAsync("/cars");
            var headers = response.Headers;

            Assert.NotEmpty(response.Headers);
            
            Assert.NotNull(response.Headers.CacheControl);
            Assert.True(response.Headers.CacheControl.MaxAge > default(TimeSpan));

            _output.WriteLine(headers.CacheControl.MaxAge.ToString());
        }

    }
}
