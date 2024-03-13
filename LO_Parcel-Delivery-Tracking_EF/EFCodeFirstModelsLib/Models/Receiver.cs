using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstModelsLib.Models
{
    [Table("Receivers")]
    public class Receiver
    {
        [Key, Column("receiver_id")]
        public int ReceiverId { get; set; }

        [Column("participant_id"), Required, ForeignKey("ParcelParticipant")]
        public int ParticipantID { get; set; }
        public virtual ParcelParticipant ParcelParticipant { get; set; }

        public virtual ICollection<Parcel> Parcels { get; set; }
    }

}
