namespace ParcelDeliveryTrackingAPI.Dto
{
    public class ParcelCreateDto
    {
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public double Weight { get; set; }
        public string ParcelStatus { get; set; } = "Parcel Delivery Confirmed";
        public DateTime? ScheduledDeliveryDate { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
