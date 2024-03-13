using Moq;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.NunitTests
{
    public class DeliveryRepoTests
    {
        private Mock<ParcelDeliveryTrackingDBContext> _mockContext;
        private Mock<DeliveryRepository> _mockDeliveryRepository;

        private List<Delivery> _deliveryList;
        private List<DeliveryDto> _deliveryListDto;
        private Delivery _delivery;

        [SetUp]
        public void Initialiser()
        {
            _mockContext = new Mock<ParcelDeliveryTrackingDBContext>();
            _mockDeliveryRepository = new Mock<DeliveryRepository>(_mockContext.Object);
            _deliveryList = new List<Delivery>();
            _deliveryListDto = new List<DeliveryDto>();

            _delivery = new Delivery
            {
                DeliveryId = 1,
                ParcelId= 1,
                PersonnelId= 1,
                DeliveryStatus = "Completed",
                DeliveryDate = DateTime.Now,

            };

            _deliveryListDto = new List<DeliveryDto>
            {
                new DeliveryDto
                {
                    DeliveryId = 1,
                    PersonnelId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    ParcelId = 1,
                    DeliveryStatus = "In Transit",
                    ParcelStatus = "In Progress",
                    ScheduledDeliveryDate = DateTime.Parse("2023-10-10"),
                    DeliveryDate = null
                },
                new DeliveryDto
                {
                    DeliveryId = 2,
                    PersonnelId = 2,
                    FirstName = "Sarah",
                    LastName = "Doe",
                    ParcelId = 2,
                    DeliveryStatus = "In Transit",
                    ParcelStatus = "In Progress",
                    ScheduledDeliveryDate = DateTime.Parse("2023-10-10"),
                    DeliveryDate = null
                },
                new DeliveryDto
                {
                    DeliveryId = 3,
                    PersonnelId = 2,
                    FirstName = "Sarah",
                    LastName = "Doe",
                    ParcelId = 3,
                    DeliveryStatus = "In Transit",
                    ParcelStatus = "In Progress",
                    ScheduledDeliveryDate = DateTime.Parse("2023-10-10"),
                    DeliveryDate = null
                },
                // Add more expected deliveries as needed
            };

            _deliveryList.Add(_delivery);
        }

        [TearDown]
        public void Cleanup()
        {
            _mockContext = null;
            _mockDeliveryRepository = null;
            _deliveryList = null;
        }


        [Test]
        public void _01Test_GetAllRows_Delivery_ReturnAllDeliveries()
        {
            // arrange
            var deliveries = new List<Delivery>
            {
                new Delivery { DeliveryId = 1, ParcelId = 1, PersonnelId= 1, DeliveryStatus = "Completed", DeliveryDate = DateTime.Now },
                new Delivery { DeliveryId = 2, ParcelId = 2, PersonnelId= 2, DeliveryStatus = "In Progress", },
                new Delivery { DeliveryId = 3, ParcelId = 3, PersonnelId= 1, DeliveryStatus = "Scheduled",  }

            };

            _mockDeliveryRepository.Setup(x => x.GetAllRows()).Returns(deliveries);

            // act
            var result = _mockDeliveryRepository.Object.GetAllRows();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(deliveries.Count));

        }

        [Test]
        public void _02Test_GetDeliveriesForPersonnel_ReturnDeliveriesForPersonnel()
        {
            // arrange
            var personnelId = 2; // Change this to the desired personnel ID for testing
            
            var personnelDeliveries = _deliveryListDto
                .Where(e => e.PersonnelId == personnelId)
                .Select(dto => new
                {
                    dto.DeliveryId,
                    dto.PersonnelId,
                    dto.FirstName,
                    dto.LastName,
                    dto.ParcelId,
                    dto.DeliveryStatus,
                    dto.ParcelStatus,
                    dto.ScheduledDeliveryDate,
                    dto.DeliveryDate
                }).ToList();


            var personnelAllDeliveries = _deliveryListDto.Where(x => x.PersonnelId == personnelId).ToList();
            _mockDeliveryRepository.Setup(d => d.GetDeliveriesForPersonnel(personnelId)).Returns(personnelAllDeliveries);

            // act
            var result = _mockDeliveryRepository.Object.GetDeliveriesForPersonnel(personnelId);


            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(personnelDeliveries.Count));

        }



        [Test]
        public void _03Test_GetRowById_ReturnEqualDeliverylById()
        {
            // arrange
            var id = 1;
            var delivery = new Delivery
            {
                DeliveryId = 1,
                ParcelId = 1,
                PersonnelId = 1,
                DeliveryStatus = "Completed",
                DeliveryDate = DateTime.Now,
            };


            _mockDeliveryRepository.Setup(m => m.GetRowById(id)).Returns(_delivery);

            // Act
            var result = _mockDeliveryRepository.Object.GetRowById(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.DeliveryId, Is.EqualTo(delivery.DeliveryId));
                Assert.That(result.ParcelId, Is.EqualTo(delivery.ParcelId));
                Assert.That(result.PersonnelId, Is.EqualTo(delivery.PersonnelId));
            });
        }

        [Test]
        public void _04Test_UpdateRow_UpdatesDeliveryStatus()
        {
            // arrange
            var delivery = new Delivery
            {
                DeliveryId = 1,
                ParcelId = 1,
                PersonnelId = 1,
                DeliveryStatus = "Completed",
                DeliveryDate = DateTime.Now,
            };
            string msg = $"Delivery with ID {delivery.DeliveryId} has been successfully updated.";

            _mockDeliveryRepository.Setup(x => x.UpdateRow(_delivery)).Returns((_delivery, msg));

            // act
            var result = _mockDeliveryRepository.Object.UpdateRow(_delivery);

            // assert
            Assert.That(result.updatedItem.DeliveryId, Is.EqualTo(delivery.DeliveryId));

            if(delivery.DeliveryStatus == "Completed")
            {
                Assert.IsNotNull(result.updatedItem.DeliveryDate);
            }
            else
            {
                Assert.IsNull(result.updatedItem.DeliveryDate);
            }

            // verify
            _mockDeliveryRepository.Verify(c => c.UpdateRow(_delivery), Times.Once);
        }


        [TestCase("In Transit")]
        [TestCase("Completed")]
        public void _05Test_GetAllDeliveriesByStatus_ReturnsDeliveriesByStatus(string status)
        {
           // arrange
            string delStatusT = "In Transit";
            string delStatusC = "Completed";
            var deliveries = new List<Delivery>
            {
                new Delivery { DeliveryId = 1, ParcelId = 1, PersonnelId= 1, DeliveryStatus = delStatusT  },
                new Delivery { DeliveryId = 2, ParcelId = 2, PersonnelId= 2, DeliveryStatus = delStatusC, DeliveryDate = new DateTime(2021,05,01,10,25,13)  },
                new Delivery { DeliveryId = 3, ParcelId = 3, PersonnelId= 1, DeliveryStatus = delStatusT  },
                new Delivery { DeliveryId = 4, ParcelId = 5, PersonnelId= 3, DeliveryStatus = delStatusC, DeliveryDate = DateTime.Now }
            };

            

            var deliveriesByStatus = deliveries.Where(d => d.DeliveryStatus == status).ToList();
            _mockDeliveryRepository.Setup(x => x.GetAllDeliveriesByStatus(status)).Returns(deliveriesByStatus);


            // act
            var result = _mockDeliveryRepository.Object.GetAllDeliveriesByStatus(status);


            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.IsTrue(result.All(d => d.DeliveryStatus == status));
        }


    }


}
