using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Address
    {
        public Address()
        {
            ParcelParticipants = new HashSet<ParcelParticipant>();
        }

        public int AddressId { get; set; }
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Suburb { get; set; }
        public string PostalCode { get; set; } = null!;

        public virtual ICollection<ParcelParticipant> ParcelParticipants { get; set; }
    }
}
