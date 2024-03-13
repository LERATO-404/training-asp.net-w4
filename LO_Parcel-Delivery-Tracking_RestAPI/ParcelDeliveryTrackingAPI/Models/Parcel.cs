using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Parcel
    {
        public Parcel()
        {
            Deliveries = new HashSet<Delivery>();
        }

        public int ParcelId { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public double Weight { get; set; }
        public string? ParcelStatus { get; set; }
        public DateTime? ScheduledDeliveryDate { get; set; }
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
