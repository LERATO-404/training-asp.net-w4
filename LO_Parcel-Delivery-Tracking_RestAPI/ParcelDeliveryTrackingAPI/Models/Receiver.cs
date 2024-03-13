using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Receiver
    {
        public Receiver()
        {
            ParcelReceiverReceivers = new HashSet<Parcel>();
            ParcelReceivers = new HashSet<Parcel>();
        }

        public int ReceiverId { get; set; }
        public int ParticipantId { get; set; }

        public virtual ParcelParticipant Participant { get; set; } = null!;
        public virtual ICollection<Parcel> ParcelReceiverReceivers { get; set; }
        public virtual ICollection<Parcel> ParcelReceivers { get; set; }
    }
}
