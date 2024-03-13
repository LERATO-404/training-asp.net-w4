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
    [Table("Deliveries")]
    public class Delivery
    {
        [Key, Column("delivery_id")]
        public int DeliveryId { get; set; }

        [Column("parcel_id"), Required, ForeignKey("Parcel")]
        public int ParcelID { get; set; }
        public virtual Parcel Parcel { get; set; }

        [Column("personnel_id"), Required, ForeignKey("Personnel")]
        public int DeliveryPersonnel { get; set; }
        public virtual Personnel Personnel { get; set; }

        [Column("delivery_status")]
        [MaxLength(50)]
        public string DeliveryStatus { get; set; }

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; }
    }
}
