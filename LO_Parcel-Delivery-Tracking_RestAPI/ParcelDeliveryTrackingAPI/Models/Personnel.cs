using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Personnel
    {
        public Personnel()
        {
            Deliveries = new HashSet<Delivery>();
        }

        public int PersonnelId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Availability { get; set; }
        public string? UserName { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
