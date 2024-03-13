namespace ParcelDeliveryTrackingAPI.Dto
{
    public class DeliveryDto
    {
        public int DeliveryId { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }


        public int PersonnelId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public int ParcelId { get; set; }
        public string ParcelStatus { get; set; } = null!;
        public DateTime? ScheduledDeliveryDate { get; set; }
    }
}
