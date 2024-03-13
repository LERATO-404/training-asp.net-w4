using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ParcelDeliveryTrackingAsp.Models
{
    public partial class Personnel
    {
        public Personnel()
        {
            Deliveries = new HashSet<Delivery>();
        }

        [Display(Name = "Personnel Id")]
        public int PersonnelId { get; set; }

        [Display(Name = "Firstname")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Lastname")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Email address")]
        public string? EmailAddress { get; set; }
        public string? Availability { get; set; }
        public string? UserName { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
