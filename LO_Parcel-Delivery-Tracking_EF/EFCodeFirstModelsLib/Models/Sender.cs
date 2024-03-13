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
    [Table("Senders")]
    public class Sender
    {
        [Key, Column("sender_id")]
        public int SenderId { get; set; }

        [Column("participant_id"), Required, ForeignKey("ParcelParticipant")]
        public int ParticipantId { get; set; }
        public virtual ParcelParticipant ParcelParticipant { get; set; }

        public string TypeOfSender { get; set; }

        public virtual ICollection<Parcel> Parcels { get; set; }
    }
}
