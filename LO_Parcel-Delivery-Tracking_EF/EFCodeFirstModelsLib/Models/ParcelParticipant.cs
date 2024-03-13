using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstModelsLib.Models
{
    [Table("Parcel_participants")]
    public class ParcelParticipant
    {
        [Key, Column("participant_id")]
        public int ParticipantId { get; set; }

        [Column("participant_name")]
        public string ParticipantName { get; set; }

        [Column("address_id"),Required, ForeignKey("Address")]
        public int AddressID { get; set; }
        public virtual Address Address { get; set; }

        [Column("phone_number"), Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Column("email_address")]
        public string EmailAddress { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        public virtual ICollection<Receiver> Receiver { get; set; }
        public virtual ICollection<Sender> Sender { get; set; }
    }
}
