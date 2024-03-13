namespace ParcelDeliveryTrackingAPI.Dto
{
    public class ParcelDto
    {
        public int ParcelId { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public double Weight { get; set; }
        public string ParcelStatus { get; set; } = null!;
        public DateTime? ScheduledDeliveryDate { get; set; }
        public string? AdditionalNotes { get; set; }

    }
}
