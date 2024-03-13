using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Helpers;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;

namespace ParcelDeliveryTrackingAPI.Repositories
{
    public class ParcelRepository : IParcel
    {
        private readonly ParcelDeliveryTrackingDBContext _parcelContext;
        

        public ParcelRepository(ParcelDeliveryTrackingDBContext parcelContext)
        {
            _parcelContext = parcelContext;
           
        }

        public Parcel CreateNewParcel(ParcelCreateDto parcelDto)
        {
            var newParcel = new Parcel()
            {
                SenderId = parcelDto.SenderId,
                ReceiverId = parcelDto.ReceiverId,
                Weight = parcelDto.Weight,
                ParcelStatus = parcelDto.ParcelStatus,
                ScheduledDeliveryDate = parcelDto.ScheduledDeliveryDate != null ? parcelDto.ScheduledDeliveryDate.Value : DateTime.Now.AddDays(7).Date.AddHours(12), // Handle null
                AdditionalNotes = parcelDto.AdditionalNotes
            };

            _parcelContext.Parcels.Add(newParcel);
            _parcelContext.SaveChanges();

            return newParcel;
        }

        public bool DeleteRow(int id)
        {
            bool flag = false;
            var parcelToDelete = _parcelContext.Parcels
                   .Find(id);

            if (parcelToDelete != null)
            {
                _parcelContext.Parcels.Remove(parcelToDelete);
                _parcelContext.SaveChanges();
                flag = true;
            }

            return flag;
         
        }

        public virtual List<ParcelDto> GetAllParcelByStatus(string status)
        {
            var lowercaseStatus = status.ToLower();

            var parcelsByStatus =  _parcelContext.Parcels
                .Where(p => p.ParcelStatus.ToLower() == lowercaseStatus)
                .Select(parcel => new ParcelDto
                {
                    ParcelId = parcel.ParcelId,
                    SenderId = parcel.SenderId,
                    ReceiverId = parcel.ReceiverId,
                    Weight = parcel.Weight,
                    ParcelStatus = parcel.ParcelStatus,
                    ScheduledDeliveryDate = parcel.ScheduledDeliveryDate,
                    AdditionalNotes = parcel.AdditionalNotes
                })
                .ToList();

            if (parcelsByStatus.Count() == 0)
            {
                return null;
            }

            return parcelsByStatus;
        }

        public virtual List<ParcelDto> GetAllRows()
        {
            var parcelDto = _parcelContext.Parcels
                .Select(p => new ParcelDto
                { 
                    ParcelId= p.ParcelId,
                    SenderId= p.SenderId,
                    ReceiverId= p.ReceiverId,
                    Weight= p.Weight,
                    ParcelStatus= p.ParcelStatus,
                    ScheduledDeliveryDate= p.ScheduledDeliveryDate,
                    AdditionalNotes= p.AdditionalNotes,
                
                }
                ).ToList();
            return parcelDto;
        }

        public ParcelDto GetRowById(int id)
        {
            ParcelDto parcelDto = new ParcelDto();
            var parcel = _parcelContext.Parcels.FirstOrDefault(p => p.ParcelId == id);

            if (parcel == null)
            {
                return null;
            }
            parcelDto.ParcelId = parcel.ParcelId;
            parcelDto.SenderId = parcel.SenderId;
            parcelDto.ReceiverId = parcel.ReceiverId;
            parcelDto.Weight = parcel.Weight;
            parcelDto.ParcelStatus = parcel.ParcelStatus;
            parcelDto.ScheduledDeliveryDate = parcel.ScheduledDeliveryDate;
            parcelDto.AdditionalNotes = parcel.AdditionalNotes;

            return parcelDto;
        }

        public List<ParcelDto> GetSenderParcels(int senderId)
        {
            var parcel =  _parcelContext.Parcels
                    .Where(p => p.SenderId == senderId)
                    .Select(parcel => new ParcelDto
                    {
                        ParcelId = parcel.ParcelId,
                        SenderId = parcel.SenderId,
                        ReceiverId = parcel.ReceiverId,
                        Weight = parcel.Weight,
                        ParcelStatus = parcel.ParcelStatus,
                        ScheduledDeliveryDate = parcel.ScheduledDeliveryDate,
                        AdditionalNotes = parcel.AdditionalNotes
                    })
                    .ToList();

            if(parcel.Count == 0)
            {
                return null;
            }

            return parcel;
        }

        public Parcel UpdateParcelInformation(int id, ParcelDto parcelDto)
        {
            var parcel = _parcelContext.Parcels.Find(id);

            if (parcel == null)
            {
                return null;
            }
            parcel.ParcelId = parcelDto.ParcelId;
            parcel.SenderId = parcelDto.SenderId;
            parcel.ReceiverId = parcelDto.ReceiverId;
            parcel.Weight = parcelDto.Weight;
            parcel.ParcelStatus = parcelDto.ParcelStatus;
            parcel.ScheduledDeliveryDate = parcelDto.ScheduledDeliveryDate;
            parcel.AdditionalNotes = parcelDto.AdditionalNotes;


            _parcelContext.SaveChanges();

            var parcelUpdated = new Parcel()
            {
                ParcelId = parcel.ParcelId,
                SenderId = parcel.SenderId,
                ReceiverId= parcel.ReceiverId,
                Weight = parcel.Weight,
                ParcelStatus= parcel.ParcelStatus,
                ScheduledDeliveryDate= parcel.ScheduledDeliveryDate,
                AdditionalNotes= parcel.AdditionalNotes
            };

            return parcelUpdated;
        }

        public (ParcelDto updatedItem, string message) UpdateRow(ParcelDto parcelDto)
        {
            
            var parcel = _parcelContext.Parcels.Find(parcelDto.ParcelId);

            if(parcel == null)
            {
                return (null, $"Parcel with ID {parcel.ParcelId} was not found. Update failed.");
            }

            parcel.SenderId = parcelDto.SenderId;
            parcel.ReceiverId = parcelDto.ReceiverId;
            parcel.Weight = parcelDto.Weight;
            parcel.ParcelStatus = parcelDto.ParcelStatus;
            parcel.ScheduledDeliveryDate = parcelDto.ScheduledDeliveryDate;
            parcel.AdditionalNotes = parcelDto.AdditionalNotes;

            _parcelContext.SaveChanges();
    
            var updateParcel = new ParcelDto
            {
                ParcelId = parcelDto.ParcelId,
                SenderId = parcelDto.SenderId,
                ReceiverId = parcelDto.ReceiverId,
                Weight = parcelDto.Weight,
                ParcelStatus = parcelDto.ParcelStatus,
                ScheduledDeliveryDate = parcelDto.ScheduledDeliveryDate,
                AdditionalNotes = parcelDto.AdditionalNotes
            };
            return (updateParcel, $"Parcel Status with ID {parcel.ParcelId} has been successfully updated.");
        }
    }
}
