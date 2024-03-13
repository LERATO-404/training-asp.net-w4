using Microsoft.AspNetCore.Mvc;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Models;

namespace ParcelDeliveryTrackingAPI.Interfaces
{
    public interface IPersonnel : IRepository<Personnel>
    {
        List<Personnel> GetPersonnelByAvailability(string availability);        // method for getting personnel by avaliablity (On Duty or Off Duty)
        Personnel GetPersonnelByFullName(string firstName, string lastName);    // method for getting personnel by full name
        Personnel CreateNewPersonnel(PersonnelDto personnel);                   // method to create a new personnel
        bool PersonnelExists(int id);                                           // method to check if a personnnel exist
        
    }
}
