using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCodeFirstModelsLib.Enums;

namespace EFCodeFirstModelsLib.Models
{
    [Table("Parcels")]
    public class Parcel
    {
        [Key, Column("parcel_id")]
        public int ParcelId { get; set; }

        [Column("sender_id")]
        public int? SenderID { get; set; }
        public virtual Sender Sender { get; set; }

        [Column("receiver_id")]
        public int? ReceiverID { get; set; }
        public virtual Receiver Receiver { get; set; }

        [Column("weight"), Required]
        public double Weight { get; set; }

        [Column("parcel_status"), Required]
        public string ParcelStatus { get; set; }

        [Column("scheduled_delivery_date")]
        public DateTime? DeliveryDate { get; set; }

        [Column("additional_notes")]
        [MaxLength(255)]
        public string AdditionalNotes { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

    }

}
