using System;
using System.Collections.Generic;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class Delivery
    {
        public int DeliveryId { get; set; }
        public int ParcelId { get; set; }
        public int PersonnelId { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual Parcel Parcel { get; set; } = null!;
        public virtual Personnel Personnel { get; set; } = null!;
    }
}
