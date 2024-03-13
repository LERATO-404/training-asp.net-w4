namespace ParcelDeliveryTrackingAPI.Dto
{
    public class PersonnelDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string Availability { get; set; } = null!;
        public string? UserName { get; set; }
    }
}
