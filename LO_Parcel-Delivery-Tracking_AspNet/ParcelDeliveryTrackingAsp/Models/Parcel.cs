using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ParcelDeliveryTrackingAsp.Models
{
    public partial class Parcel
    {
        public Parcel()
        {
            Deliveries = new HashSet<Delivery>();
        }
        [Display(Name = "Parcel Id")]
        public int ParcelId { get; set; }

        [Display(Name = "Sender Id")]
        public int? SenderId { get; set; }

        [Display(Name = "Receiver Id")]
        public int? ReceiverId { get; set; }

       
        public double Weight { get; set; }

        [Display(Name = "Parcel status")]
        public string? ParcelStatus { get; set; }

        [Display(Name = "Scheduled delivery date")]
        public DateTime? ScheduledDeliveryDate { get; set; }

        [Display(Name = "Additional notes")]
        public string? AdditionalNotes { get; set; }
        public int? SenderSenderId { get; set; }
        public int? ReceiverReceiverId { get; set; }

        public virtual Receiver? Receiver { get; set; }
        public virtual Receiver? ReceiverReceiver { get; set; }
        public virtual Sender? Sender { get; set; }
        public virtual Sender? SenderSender { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
