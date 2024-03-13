using Microsoft.AspNetCore.Mvc;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Models;

namespace ParcelDeliveryTrackingAPI.Interfaces
{
    public interface IDelivery : IRepository<Delivery>
    {
        List<Delivery> GetAllDeliveriesByStatus(string status);                         // method to get all deliveries by status
        List<DeliveryDto> GetDeliveriesForPersonnel(int personnelId);                   // method to get deliveries for the specified personnel
        Delivery UpdateDeliveryStatus(int id, DeliveryStatusDto deliveryStatusDto);     // nethod to update delivery status

        Delivery CreateNewDelivery(DeliveryCreateDto deliveryDto);                               // method to create a new delivery
    }
}
