using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Sender
    {
        public Sender()
        {
            ParcelSenderSenders = new HashSet<Parcel>();
            ParcelSenders = new HashSet<Parcel>();
        }

        public int SenderId { get; set; }
        public int ParticipantId { get; set; }
        public string? TypeOfSender { get; set; }

        public virtual ParcelParticipant Participant { get; set; } = null!;
        public virtual ICollection<Parcel> ParcelSenderSenders { get; set; }
        public virtual ICollection<Parcel> ParcelSenders { get; set; }
    }
}
