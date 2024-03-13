using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ParcelDeliveryTrackingAsp.Areas.AuthServices.Models
{
    public class LoginVM
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [StringLength(60, MinimumLength = 8)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
