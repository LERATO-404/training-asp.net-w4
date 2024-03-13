using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryTrackingAsp.NetMVC.xUnitTests
{
    public class AdmCrudPersonnelControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _httpClient;

        public AdmCrudPersonnelControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory;
            _httpClient = _applicationFactory.CreateClient();
        }

        [Theory]
        [InlineData("Admin/CrudPersonnel/Index")]
        [InlineData("Admin/CrudPersonnel/Update/2")]
        [InlineData("Admin/CrudPersonnel/Details/1")]
        [InlineData("Admin/CrudPersonnel/Delete/3")]
        public async void _01_Test_All_Personnel_URL_ReturnsOkResponse(string URL)
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
        [InlineData("David")]
        [InlineData("david@example.com")]
        [InlineData("David.Johnson")]
        public async void _2_Test_Personnel_Details_Data_ReturnContent(string item)
        {

            // act
            var response = await _httpClient.GetAsync("Admin/CrudPersonnel/Details/1");
            var pageContent = await response.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            // assert
            Assert.Contains(item, contentString);
        }

        [Theory]
        [InlineData("Davids")]
        [InlineData("david@@example.com")]
        [InlineData("DavidJohnson")]
        public async void _3_Test_Personnel_Details_Data_DoesNotReturnContent(string item)
        {

            // act
            var response = await _httpClient.GetAsync("Admin/CrudPersonnel/Details/1");
            var pageContent = await response.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            // assert
            Assert.DoesNotContain(item, contentString);
        }
    }
}
