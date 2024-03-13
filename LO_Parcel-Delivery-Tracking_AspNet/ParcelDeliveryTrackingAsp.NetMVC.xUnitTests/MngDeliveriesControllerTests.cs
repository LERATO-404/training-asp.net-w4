using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryTrackingAsp.NetMVC.xUnitTests
{
    public class MngDeliveriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _httpClient;

        public MngDeliveriesControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory;
            _httpClient = _applicationFactory.CreateClient();
        }

        [Theory]
        [InlineData("Manager/ManagerDeliveries/Index")]
        [InlineData("Manager/ManagerDeliveries/Details/1")]
        public async void _01_Test_All_Actions_Deliveries_URL_ReturnsOkResponse(string URL)
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

       /* [Fact]
        public async Task _2_Test_Create_Get_ReturnsViewResult()
        {
            // arrange
            var client = _applicationFactory.CreateClient();

            // act
            var response = await client.GetAsync("/Manager/ManagerDeliveries/Create");
            int statusCode = (int)response.StatusCode;


            // assert
            Assert.Equal(200, statusCode);
            Assert.IsType<ViewResult>(response);

        }*/


    }
}
