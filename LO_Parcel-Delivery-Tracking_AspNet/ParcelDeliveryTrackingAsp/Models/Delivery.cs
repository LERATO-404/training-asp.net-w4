using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ParcelDeliveryTrackingAsp.Models
{
    public partial class Delivery
    {
        public int DeliveryId { get; set; }

        [Display(Name = "Parcel Id")]
        public int ParcelId { get; set; }

        [Display(Name = "Personnel Id")]
        public int PersonnelId { get; set; }

        [Display(Name = "Delivery status")]
        public string? DeliveryStatus { get; set; }

        [Display(Name = "Delivery date")]
        public DateTime? DeliveryDate { get; set; }

        public virtual Parcel Parcel { get; set; } = null!;
        public virtual Personnel Personnel { get; set; } = null!;
    }
}
