using Microsoft.AspNetCore.Mvc.Testing;
using ParcelDeliveryTrackingAsp;
using System.Net;

namespace ParcelDeliveryTrackingAsp.NetMVC.xUnitTests
{
    public class AdmCrudDeliveriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _httpClient;

        public AdmCrudDeliveriesControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory;
            _httpClient = _applicationFactory.CreateClient();
        }

        [Theory]
        [InlineData("Admin/CrudDeliveries/Index")]
        [InlineData("Admin/CrudDeliveries/Create")]
        [InlineData("Admin/CrudDeliveries/Details/1")]
        [InlineData("Admin/CrudDeliveries/Update/2")]
        [InlineData("Admin/CrudDeliveries/Delete/3")]
        [InlineData("Admin/CrudDeliveries/DeliveriesByStatus/Completed")]
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


        [Theory]
        [InlineData("In Progress")]
        [InlineData("2")]
        public async void _2_Test_Delivery_Details_Data_ReturnContent(string item)
        {

            // act
            var response = await _httpClient.GetAsync("Admin/CrudDeliveries/Details/1");
            var pageContent = await response.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(item, contentString);
        }
       
    }
}