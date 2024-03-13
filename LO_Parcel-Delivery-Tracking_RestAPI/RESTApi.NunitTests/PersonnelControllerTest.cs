using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Controllers;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class PersonnelControllerTest
    {

        Object _parcelContext;
        private PersonnelController _controllerUnderTest;
        private List<Personnel> _personnelList;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<AuthenticationContext> _authContext;

        Personnel _personnel;
        PersonnelDto _personnelDto;

        [SetUp]
        public void Initialiser()
        {
            _parcelContext = InMemoryContext.GeneratedParcels();

            _personnelList = new List<Personnel>();
            _personnel = new Personnel()
            {
               PersonnelId = 1,
               FirstName= "Test",
               LastName= "Test",
               PhoneNumber = "000-000-999",
               EmailAddress = "Test@gmail.com",
               Availability = "On duty",
               UserName = "Test",
            };
            _personnelDto = new PersonnelDto()
            {
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "000-000-999",
                EmailAddress = "Test@gmail.com",
                Availability = "On duty",
                UserName = "Test",
            };
        }

        [TearDown]
        public void Cleanup()
        {
            _personnelList = null;
            _personnel = null;
            _personnelDto = null;
        }

        [Test]
        public async Task _01_Test_GetAllPersonnel_ReturnAListWithValidCount()
        {
           /* var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _controllerUnderTest = new PersonnelController(_localParcelContext);

            await _controllerUnderTest.PostPersonnel(_personnelDto);
            await _controllerUnderTest.PostPersonnel(new PersonnelDto());
            await _controllerUnderTest.PostPersonnel(_personnelDto);
            await _controllerUnderTest.PostPersonnel(new PersonnelDto());

            // act
            var result = await _controllerUnderTest.GetPersonnelById(1);


            // assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionResult<Personnel>>(result);*/

        }

    }
}
