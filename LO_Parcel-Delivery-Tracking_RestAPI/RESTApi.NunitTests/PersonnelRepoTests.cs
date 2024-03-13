using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol.Core.Types;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class PersonnelRepoTests
    {

        private Mock<ParcelDeliveryTrackingDBContext> _mockContext;
        private Mock<PersonnelRepository> _mockPersonnelRepository;

        private List<Personnel> _personnelList;
        private Personnel _personnel;

        [SetUp]
        public void Initialiser()
        {
            _mockContext = new Mock<ParcelDeliveryTrackingDBContext>();
            _mockPersonnelRepository = new Mock<PersonnelRepository>(_mockContext.Object);
            _personnelList = new List<Personnel>();

            _personnel = new Personnel
            {
                PersonnelId = 1,
                FirstName = "George",
                LastName = "Johnson",
                PhoneNumber = "1234567890",
                EmailAddress = "GeorgeJ@gmail.com",
                Availability = "On Duty",
                UserName = "GeorgeJohnson"
            };

            _personnelList.Add(_personnel);
        }

        [TearDown]
        public void Cleanup()
        {
            _mockContext = null;
            _mockPersonnelRepository = null;
            _personnelList = null;
        }

        [Test]
        public void _01Test_GetAllRows_Personnel_IsCalledOnce()
        {
         
            //Act
            _personnelList = _mockPersonnelRepository.Object.GetAllRows();

            //Assert
            _mockPersonnelRepository.Verify(n => n.GetAllRows(), Times.Once);
        }

        [Test]
        public void _02Test_GetAllRows_ReturnAllPersonnel()
        {
            // arrange
           
            _mockPersonnelRepository.Setup(x => x.GetAllRows()).Returns(_personnelList);

            // act
            var personnelList = _mockPersonnelRepository.Object.GetAllRows();

            // assert
            Assert.That(personnelList, Is.Not.Null);
            Assert.That(personnelList.Count(), Is.EqualTo(_personnelList.Count()));

        }

        [Test]
        public void _03Test_GetAllRows_ReturnsEmptyList()
        {
            // arrange
            _mockPersonnelRepository.Setup(x => x.GetAllRows()).Returns(_personnelList);

            // act
            var actual = _mockPersonnelRepository.Object.GetAllRows();
            var expected = _personnelList;

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }


        [Test]
        public void _04Test_GetPersonnelByFullName_ReturnMatchingPersonnel()
        {
            // arrange
            string firstName = "George";
            string lastName = "Johnson";

            _mockPersonnelRepository.Setup(x => x.GetPersonnelByFullName(firstName, lastName)).Returns(_personnelList.First());

            // act
            var personnel = _mockPersonnelRepository.Object.GetPersonnelByFullName(firstName, lastName);

            // assert
            
            Assert.That(personnel, Is.Not.Null);
            Assert.That(personnel.FirstName, Is.EqualTo(firstName));
            Assert.That(personnel.LastName, Is.EqualTo(lastName));
        }

        [Test]
        public void _05Test_DeleteRow_DeletedPersnnelIfExists()
        {
            // arrange
            var id = 1;

            _mockPersonnelRepository.Setup(m => m.DeleteRow(id)).Returns(true);

            // Act
            var result = _mockPersonnelRepository.Object.DeleteRow(id);

            // Assert
            Assert.IsTrue(result); // The personnel was found and deleted
            _mockPersonnelRepository.Verify(c => c.DeleteRow(id), Times.Once);
        }




        [Test]
        public void _06Test_CreateNewPersonnel_AddsPersonnelToContext()
        {
            // arrange

            var newPersonnelDto = new PersonnelDto
           {
               FirstName = "Jane",
               LastName = "Smith",
               PhoneNumber = "987-654-3210",
               EmailAddress = "jane.smith@example.com",
               Availability = "Part-Time",
               UserName = "janesmith"
            };

            _mockPersonnelRepository.Setup(x => x.CreateNewPersonnel(It.IsAny<PersonnelDto>()))
            .Returns<PersonnelDto>((personnelDto) =>
            {
                var newPersonnel = new Personnel
                {
                    FirstName = personnelDto.FirstName,
                    LastName = personnelDto.LastName,
                    PhoneNumber = personnelDto.PhoneNumber,
                    EmailAddress = personnelDto.EmailAddress,
                    Availability = personnelDto.Availability,
                    UserName = personnelDto.UserName
                };

                return newPersonnel;
            });

            // act
            var result = _mockPersonnelRepository.Object.CreateNewPersonnel(newPersonnelDto);


            // assert
            Assert.That(result, Is.Not.Null); // Ensure that the result is not null
            Assert.Multiple(() =>
            {
                Assert.That(result.FirstName, Is.EqualTo(newPersonnelDto.FirstName));
                Assert.That(result.LastName, Is.EqualTo(newPersonnelDto.LastName));
                Assert.That(result.PhoneNumber, Is.EqualTo(newPersonnelDto.PhoneNumber));
                Assert.That(result.EmailAddress, Is.EqualTo(newPersonnelDto.EmailAddress));
                Assert.That(result.Availability, Is.EqualTo(newPersonnelDto.Availability));
                Assert.That(result.UserName, Is.EqualTo(newPersonnelDto.UserName));
            });

            // Verify that CreateNewPersonnel was called with the provided personnelDto
            _mockPersonnelRepository.Verify(x => x.CreateNewPersonnel(newPersonnelDto), Times.Once);
        }


        [Test]
        public void _07Test_CreateNewPersonnel_DoesNotAddPersonnelToContext()
        {
            // arrange

            var newPersonnelDto = new PersonnelDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "987-654-3210",
                EmailAddress = "jane.smith@example.com",
                Availability = "On Duty",
                UserName = "janesmith"
            };

            _mockPersonnelRepository.Setup(x => x.CreateNewPersonnel(It.IsAny<PersonnelDto>()))
            .Returns<PersonnelDto>((personnelDto) =>
            {
                var newPersonnel = new Personnel
                {
                    FirstName = personnelDto.FirstName,
                    LastName = personnelDto.LastName,
                    PhoneNumber = personnelDto.PhoneNumber,
                    EmailAddress = personnelDto.EmailAddress,
                    Availability = personnelDto.Availability,
                    UserName = personnelDto.UserName
                };

                return newPersonnel;
            });

            // act
            var result = _mockPersonnelRepository.Object.CreateNewPersonnel(newPersonnelDto);


            // assert
            Assert.That(result, Is.Not.Null); // Ensure that the result is not null
            Assert.Multiple(() =>
            {
                Assert.That(result.PhoneNumber, Is.Not.EqualTo("333-555-4444"));
                Assert.That(result.EmailAddress, Is.Not.EqualTo("janny.smith@example.com"));
                Assert.That(result.UserName, Is.Not.EqualTo("JannySmith"));
            });

            // Verify that CreateNewPersonnel was called with the provided personnelDto
            _mockPersonnelRepository.Verify(x => x.CreateNewPersonnel(newPersonnelDto), Times.Once);
        }



        [Test]
        public void _08Test_GetPersonnelByAvailability_ReturnsPersonnelDuty()
        {
            // assign
            string availabilityOn = "On Duty";
            string availabilityOff = "Off Duty";
            var personnelData = new List<Personnel>
            {
                new Personnel { PersonnelId = 1, FirstName="John", LastName = "Cook", PhoneNumber="098-978-7677", EmailAddress="JohnCo@gmail.com", Availability = availabilityOn, UserName="JohnCook" },
                new Personnel { PersonnelId = 2, FirstName="John", LastName = "Goody", PhoneNumber="123-345-7677", EmailAddress="JohnGo@gmail.com", Availability = availabilityOn, UserName="JohnCook" },
                new Personnel { PersonnelId = 3, FirstName="Monny", LastName = "Tail", PhoneNumber="877-978-7677", EmailAddress="Mony@gmail.com", Availability = availabilityOff, UserName="MonnyTail" }
            };


            var personnelOnDuty = personnelData.Where(personnel => personnel.Availability == availabilityOn).ToList();
            _mockPersonnelRepository.Setup(x => x.GetPersonnelByAvailability(availabilityOn)).Returns(personnelOnDuty);

            // Act
            var result = _mockPersonnelRepository.Object.GetPersonnelByAvailability(availabilityOn).Count();
            
            
            // Assert
            Assert.That(result, Is.EqualTo(2)); // There are two personnel with "On Duty" availability
        }

        

        [Test]
        public void _09Test_PersonnelExists_ReturnsTrueForExistingId()
        {
            // arrange
            var id = 2;
          
            _mockPersonnelRepository.Setup(x => x.PersonnelExists(id)).Returns(true);
            
            // act
            var result = _mockPersonnelRepository.Object.PersonnelExists(id);

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void _10Test_PersonnelExists_ReturnsFalseFotNotExistingId()
        {
            // arrange
            var id = 2;
            
            _mockPersonnelRepository.Setup(x => x.PersonnelExists(id)).Returns(false);

            // act
            var result = _mockPersonnelRepository.Object.PersonnelExists(1);

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void _11Test_GetRowById_ReturnEqualPersonnelById()
        {
            // arrange
            var id = 1;
            var personnel = new Personnel
            {
                PersonnelId = id,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "098-978-7677",
                EmailAddress = "JohnCo@gmail.com",
                Availability = "On Duty",
                UserName = "JohnCook"
            };

            
            _mockPersonnelRepository.Setup(m => m.GetRowById(id)).Returns(_personnel);

            // Act
            var result = _mockPersonnelRepository.Object.GetRowById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.PersonnelId, Is.EqualTo(id));
                Assert.That(result.FirstName, Is.EqualTo(_personnel.FirstName));
                Assert.That(result.LastName, Is.EqualTo(_personnel.LastName));
            });
        }

        [Test]
        public void _12Test_GetRowById_ReturnNotEqualPersonnelById()
        {
            // arrange
            var id = 2;
            var personnel = new Personnel
            {
                PersonnelId = id,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "098-978-7677",
                EmailAddress = "JohnCo@gmail.com",
                Availability = "On Duty",
                UserName = "JohnCook"
            };

            _mockPersonnelRepository.Setup(m => m.GetRowById(id)).Returns(personnel);

            // Act
            var result = _mockPersonnelRepository.Object.GetRowById(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.PersonnelId, Is.Not.EqualTo(_personnel.PersonnelId));
                Assert.That(result.PhoneNumber, Is.Not.EqualTo(_personnel.PhoneNumber));
                Assert.That(result.EmailAddress, Is.Not.EqualTo(_personnel.EmailAddress));
                Assert.That(result.UserName, Is.Not.EqualTo(_personnel.UserName));
            });
        }

        [Test]
        public void _13Test_UpdateRow_UpdatesPersonnel()
        {
            // arrange
           
            var personnel = new Personnel
            {
                PersonnelId = 1,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "098-978-7677",
                EmailAddress = "JohnCo@gmail.com",
                Availability = "On Duty",
                UserName = "JohnDoe"
            };
            string msg = $"Personnel with ID {personnel.PersonnelId} has been successfully updated.";

            _mockPersonnelRepository.Setup(x => x.UpdateRow(_personnel)).Returns((_personnel, msg));

            // act
            var results = _mockPersonnelRepository.Object.UpdateRow(_personnel);

            // assert
            Assert.That(results.updatedItem.PersonnelId, Is.EqualTo(personnel.PersonnelId));

            // verify
            _mockPersonnelRepository.Verify(c => c.UpdateRow(_personnel), Times.Once);
        }

        [TestCase("On Duty")]
        [TestCase("Off Duty")]
        public void _14Test_GetPersonnelByAvailability_ReturnsPersonnelDuty(string availabilityStatus)
        {
            // assign
            string availabilityOn = "On Duty";
            string availabilityOff = "Off Duty";
            var personnelData = new List<Personnel>
            {
                new Personnel { PersonnelId = 1, FirstName="John", LastName = "Cook", PhoneNumber="098-978-7677", EmailAddress="JohnCo@gmail.com", Availability = availabilityOn, UserName="JohnCook" },
                new Personnel { PersonnelId = 2, FirstName="John", LastName = "Goody", PhoneNumber="123-345-7677", EmailAddress="JohnGo@gmail.com", Availability = availabilityOn, UserName="JohnCook" },
                new Personnel { PersonnelId = 3, FirstName="Monny", LastName = "Tail", PhoneNumber="877-978-7677", EmailAddress="Mony@gmail.com", Availability = availabilityOff, UserName="MonnyTail" },
                new Personnel { PersonnelId = 3, FirstName="Tomas", LastName = "Goody", PhoneNumber="877-566-7777", EmailAddress="TomG@gmail.com", Availability = availabilityOff, UserName="TGoody" }
            };


            var personnelByAvailability = personnelData.Where(personnel => personnel.Availability == availabilityStatus).ToList();
            _mockPersonnelRepository.Setup(x => x.GetPersonnelByAvailability(availabilityStatus)).Returns(personnelByAvailability);

            // Act

            var result = _mockPersonnelRepository.Object.GetPersonnelByAvailability(availabilityStatus).Count();


            // Assert
            Assert.That(result, Is.EqualTo(2)); // There are two personnel with "On Duty" availability
        }



        [TestCase("John", "Doe", true)]
        [TestCase("Jane", "Smith", false)]
        [TestCase("Alice", "Johnson", false)]
        public void _15Test_GetPersonnelByFullName_ReturnsMatchingPersonnel(string firstName, string lastName, bool expectedResult)
        {
            // arrange
            var personnelData = new List<Personnel>
            {
                new Personnel { PersonnelId = 1, FirstName="John", LastName = "Doe", PhoneNumber="098-978-7677", EmailAddress="JohnCo@gmail.com", Availability = "On Duty", UserName="JohnCook" },
                new Personnel { PersonnelId = 2, FirstName="Jane", LastName = "Smth", PhoneNumber="123-345-7677", EmailAddress="Jane@gmail.com", Availability = "On Duty", UserName="JohnCook" },
                new Personnel { PersonnelId = 3, FirstName="Elice", LastName = "Johnson", PhoneNumber="877-978-7677", EmailAddress="Elice@gmail.com", Availability = "Off Duty", UserName="MonnyTail" }
            };


            _mockPersonnelRepository.Setup(x => x.GetPersonnelByFullName(firstName, lastName))
                           .Returns(personnelData.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName));


            // act
            var personnel = _mockPersonnelRepository.Object.GetPersonnelByFullName(firstName, lastName);

            // assert
            Assert.That(personnel != null, Is.EqualTo(expectedResult));
        }
    }
}
