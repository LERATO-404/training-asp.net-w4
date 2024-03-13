using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCodeFirstModelsLib.Models
{
    [Table("Address")]
    public class Address
    {
        [Key, Column("address_id")]
        public int AddressID { get; set; }
        [Column("address_line"), Required]
        [MaxLength(100)]
        public string AddressLine { get; set; }

        [Column("city")]
        [MaxLength(100)]
        public string City { get; set; }

        [Column("suburb")]
        [MaxLength(100)]
        public string Suburb { get; set; }

        [Required]
        [Column("postal_code")]
        [MaxLength(50)]
        public string PostalCode { get; set; }

        public virtual ICollection<ParcelParticipant> ParcelParticipants { get; set; }
    }
}
