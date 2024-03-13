using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryTrackingAsp.NetMVC.xUnitTests
{
    public class DrvDeliveriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _httpClient;

        public DrvDeliveriesControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory;
            _httpClient = _applicationFactory.CreateClient();
        }

        [Theory]
        [InlineData("Driver/DriverDeliveries/Index")]
        [InlineData("Driver/DriverDeliveries/Update/1")]
        public async void _01_Test_All_Deliveries_URL_ReturnsOkResponse(string URL)
        {
            // arrange
            var client = _applicationFactory.CreateClient();

            // act
            var response = await client.GetAsync(URL);
            int statusCode = (int)response.StatusCode;

            // assert
            Assert.NotNull(response);
            Assert.Equal(200, statusCode);
        }


    }
}
