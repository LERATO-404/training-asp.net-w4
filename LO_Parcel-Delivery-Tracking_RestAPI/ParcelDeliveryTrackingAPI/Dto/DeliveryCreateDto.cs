namespace ParcelDeliveryTrackingAPI.Dto
{
    public class DeliveryCreateDto
    {
        public int ParcelId { get; set; }
        public int PersonnelId { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
