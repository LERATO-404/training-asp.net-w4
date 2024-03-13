using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Models;

namespace ParcelDeliveryTrackingAPI.Interfaces
{
    public interface IParcel : IRepository<ParcelDto>
    {
        List<ParcelDto> GetAllParcelByStatus(string status);    // method for getting all the parcels for the specified stust (e.g., Delivered, In Progress)
        List<ParcelDto> GetSenderParcels(int senderId);         // method to get all parcels for the specified sender
        Parcel UpdateParcelInformation(int id, ParcelDto parcel);     // nethod to update delivery status
        Parcel CreateNewParcel(ParcelCreateDto parcelDto);       // method to create a new parcel
    }
}
