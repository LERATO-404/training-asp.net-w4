using ParcelDeliveryTrackingAPI.Controllers;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;
using ParcelDeliveryTrackingAPI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParcelDeliveryTrackingAPI.Interfaces;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class ParcelRepoTests
    {
        object _parcelContext;
        private ParcelRepository _repositoryUnderTest;
        private List<Parcel> _parcelList;

        private ParcelCreateDto _parcelDto;
        private ParcelCreateDto _parcelDto1;
        private ParcelCreateDto _parcelDto2;
        private Parcel _parcel;

        [SetUp]
        public void Initialiser()
        {
            _parcelContext = InMemoryContext.GeneratedParcels();
            _parcelList = new List<Parcel>();
            _parcel = new Parcel()
            {
                ParcelId = 1,
                SenderId = 1,
                ReceiverId = 1,
                Weight = 1.34,
                ParcelStatus = "In Progress",
                ScheduledDeliveryDate = DateTime.Now.AddDays(7).Date.AddHours(12),
                AdditionalNotes = "Handle with care."

            };

            _parcelDto = new ParcelCreateDto()
            {
                SenderId = 1,
                ReceiverId = 1,
                Weight = 1.34,
                ParcelStatus = "In Progress",
                ScheduledDeliveryDate = DateTime.Now.AddDays(7).Date.AddHours(12),
                AdditionalNotes = "Test parcel 1."

            };

            _parcelDto1 = new ParcelCreateDto
            {
                SenderId = 2,
                ReceiverId = 3,
                Weight = 1.5,
                ParcelStatus = "In Transit",
                ScheduledDeliveryDate = DateTime.Now.AddDays(7).Date.AddHours(12),
                AdditionalNotes = "Test parcel 2"
            };

            _parcelDto2 = new ParcelCreateDto
            {
                SenderId = 2,
                ReceiverId = 1,
                Weight = 12.2,
                ParcelStatus = "Completed",
                ScheduledDeliveryDate = DateTime.Now.AddDays(7).Date.AddHours(12),
                AdditionalNotes = "Test parcel 3"
            };
        }

        [TearDown]
        public void Cleanup()
        {
            _parcelList = null;
            _parcel = null;
            _parcelDto = null;
            _parcelDto1 = null;
            _parcelDto2 = null;
        }


        [Test]
        public void _01Test_GetAllRows_ReturnsAListOfParcelsWthValidCount()
        {
            //Arrange 
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);


            //Act
            _repositoryUnderTest.CreateNewParcel(new ParcelCreateDto());
            _repositoryUnderTest.CreateNewParcel(new ParcelCreateDto());
            


            //Assert
            Assert.NotNull(_localParcelContext.Parcels);
            Assert.That(2, Is.EqualTo(_localParcelContext.Parcels.Count()));

        }

        [Test]
        public void _02Test_CreateNewParcel_AddsParcelToContext()
        {
            // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

            // act
            var newParcel = _repositoryUnderTest.CreateNewParcel(_parcelDto);

            // assert
            Assert.That(newParcel, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newParcel.SenderId, Is.EqualTo(_parcelDto.SenderId));
                Assert.That(newParcel.ReceiverId, Is.EqualTo(_parcelDto.ReceiverId));
                Assert.That(newParcel.Weight, Is.EqualTo(_parcelDto.Weight));
                Assert.That(newParcel.ParcelStatus, Is.EqualTo(_parcelDto.ParcelStatus));
                Assert.That(newParcel.ScheduledDeliveryDate, Is.EqualTo(_parcelDto.ScheduledDeliveryDate));
                Assert.That(newParcel.AdditionalNotes, Is.EqualTo(_parcelDto.AdditionalNotes));
            });
        }

        [Test]
        public void _03Test_DeleteRow_RemoveParcelFromContext()
        {
           
            // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

            var newParcel = _repositoryUnderTest.CreateNewParcel(_parcelDto);

            // act
            var result = _repositoryUnderTest.DeleteRow(newParcel.ParcelId);

            // assert
            Assert.IsTrue(result);

            var deletedParcel = _localParcelContext.Parcels.Find(newParcel.ParcelId);
            Assert.IsNull(deletedParcel);
        }

        [Test]
        public void _04Test_GetAllParcelByStatus_ReturnsListOfParcelsWithValidStatus()
        {
            // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

            var parcel = _repositoryUnderTest.CreateNewParcel(_parcelDto);
            var parcel1 = _repositoryUnderTest.CreateNewParcel(_parcelDto1);


            // act
            var parcelsByStatus = _repositoryUnderTest.GetAllParcelByStatus("In Transit");

            // assert
            Assert.IsNotNull(parcelsByStatus);
            Assert.That(parcelsByStatus.Count, Is.EqualTo(1));
            Assert.That(parcelsByStatus[0].ParcelId, Is.EqualTo(parcel1.ParcelId));
            Assert.That(parcelsByStatus[0].ParcelStatus, Is.EqualTo(_parcelDto1.ParcelStatus));

        }

        [Test]
        public void _05Test_GetAllRows_ReturnsListOfAllParcels()
        {
            // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

            var parcel = _repositoryUnderTest.CreateNewParcel(_parcelDto);
            var parcel1 = _repositoryUnderTest.CreateNewParcel(_parcelDto1);

            // act
            var allParcels = _repositoryUnderTest.GetAllRows();

            // assert
            Assert.IsNotNull(allParcels);
            Assert.That(allParcels.Count, Is.EqualTo(2));
          
        }

        [Test]
        public void _06Test_GetRowById_ReturnsParcelWithValidId()
        {
            // arrange
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

   
            var parcel = _repositoryUnderTest.CreateNewParcel(_parcelDto);

            // act
            var retrievedParcel = _repositoryUnderTest.GetRowById(parcel.ParcelId);

            // assert
            Assert.IsNotNull(retrievedParcel);
            Assert.That(retrievedParcel.ParcelId, Is.EqualTo(parcel.ParcelId));
            Assert.That(retrievedParcel.ParcelStatus, Is.EqualTo(_parcelDto.ParcelStatus));
        }

        [Test]
        public void _07Test_GetSenderParcels_ReturnListOfParcelsForSender()
        {
            // arrange
            int senderId = 2;
            var _localParcelContext = (ParcelDeliveryTrackingDBContext)_parcelContext;
            _localParcelContext.Database.EnsureDeleted();
            _repositoryUnderTest = new ParcelRepository(_localParcelContext);

            var parcel1 = _repositoryUnderTest.CreateNewParcel(_parcelDto);
            var parcel2 = _repositoryUnderTest.CreateNewParcel(_parcelDto1);
            var parcel3 = _repositoryUnderTest.CreateNewParcel(_parcelDto2);

            // act
            var senderParcels = _repositoryUnderTest.GetSenderParcels(senderId);

            // assert
            Assert.IsNotNull(senderParcels);
            Assert.That(senderParcels.Count, Is.EqualTo(2));
        }
    }
}
