using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstModelsLib.Models
{
    public class Personnel
    {
        [Key, Column("personnel_id")]
        public int PersonnelId { get; set; }

        [Column("first_name"), Required(ErrorMessage = "First name is required.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Column("last_name"), Required(ErrorMessage = "First name is required.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Column("email_address")]
        public string EmailAddress { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }
        

        [Column("availability")]
        public string Availability { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public string GetPersonnelDetails()
        {
            string personnelDetails = $"{PersonnelId,-22} {FirstName,-21} {LastName,-19} {PhoneNumber,-22}" +
                $"{EmailAddress,-23} {UserName,-24} {Availability,-20}";
            return personnelDetails;
        }


    }
}
