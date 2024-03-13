using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Controllers;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Models;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class DeliveriesControllerTest
    {

        Object _parcelContext;
        private DeliveriesController _controllerUnderTest;
        private List<Delivery> _deliveryList;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<AuthenticationContext> _authContext;

        Delivery _delivery;
        DeliveryCreateDto _deliveryCreateDto;


        [SetUp]
        public void Initialiser()
        {
            _parcelContext = InMemoryContext.GeneratedParcels();
            _deliveryList = new List<Delivery>();
            _delivery = new Delivery()
            {
                DeliveryId = 1,
                ParcelId = 1,
                PersonnelId = 1,
                DeliveryStatus = "Completed",
                DeliveryDate = DateTime.Now,
            };
            _deliveryCreateDto = new DeliveryCreateDto()
            {
                ParcelId = 2,
                PersonnelId = 2,
                DeliveryStatus = "In Progress",
                DeliveryDate = DateTime.Now,
            };
        }

        [TearDown]
        public void Cleanup()
        {
            _deliveryList = null;
            _deliveryCreateDto = null;
            _delivery = null;
        }



        [Test]
        public async Task _01_Test_GetAllDeliveries_ReturnAListWithValidCount()
        {

          /*  // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            
            _localParcelContext.Database.EnsureDeleted();
            _controllerUnderTest = new DeliveriesController(_localParcelContext, _userManager,_authContext,_roleManager);

            // act
            await _controllerUnderTest.PostDelivery(new DeliveryCreateDto());
            await _controllerUnderTest.PostDelivery(new DeliveryCreateDto());
            await _controllerUnderTest.PostDelivery(new DeliveryCreateDto());

            //assert
            Assert.NotNull(_localParcelContext.Deliveries);
            Assert.AreEqual(_localParcelContext.Deliveries.Count(), 3);*/
        }


        [Test]
        public async Task _02Test_GetDe_ReturnsWithCorrectType()
        {
           /* // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _controllerUnderTest = new DeliveriesController(_localParcelContext, _userManager, _authContext, _roleManager);

            await _controllerUnderTest.PostDelivery(_deliveryCreateDto);
            await _controllerUnderTest.PostDelivery(new DeliveryCreateDto());

            // act
            var result = await _controllerUnderTest.GetDeliveryById(1);


            // assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionResult<Delivery>>(result);*/

        }

        [Test]
        public async Task _03_Test_GetDeliveriesById_AddedSuccessfullyAndShowsInContextCount()
        {
            // arrange
            /*var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _controllerUnderTest = new DeliveriesController(_localParcelContext, _userManager, _authContext, _roleManager);

            // act
            var result = await _controllerUnderTest.PostDelivery(_deliveryCreateDto);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(_localParcelContext.Deliveries.Count(), 1);*/

        }


      
    }
}