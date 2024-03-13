using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ParcelDeliveryTrackingAsp.Models
{
    public class ParcelDto
    {
        [Display(Name = "Parcel Id")]
        public int ParcelId { get; set; }

        [Display(Name = "Sender Id")]
        public int? SenderId { get; set; }

        [Display(Name = "Receiver Id")]
        public int? ReceiverId { get; set; }

        public double Weight { get; set; }

        [Display(Name = "Parcel status")]
        public string ParcelStatus { get; set; } = null!;

        [Display(Name = "Scheduled delivery date")]
        public DateTime? ScheduledDeliveryDate { get; set; }

        [Display(Name = "Additional notes")]
        public string? AdditionalNotes { get; set; }

    }
}
