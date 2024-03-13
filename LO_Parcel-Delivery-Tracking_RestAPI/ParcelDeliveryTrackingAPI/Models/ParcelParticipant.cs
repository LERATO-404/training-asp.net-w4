using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class ParcelParticipant
    {
        public ParcelParticipant()
        {
            Receivers = new HashSet<Receiver>();
            Senders = new HashSet<Sender>();
        }

        public int ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
        public int AddressId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? UserName { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual ICollection<Receiver> Receivers { get; set; }
        public virtual ICollection<Sender> Senders { get; set; }
    }
}
