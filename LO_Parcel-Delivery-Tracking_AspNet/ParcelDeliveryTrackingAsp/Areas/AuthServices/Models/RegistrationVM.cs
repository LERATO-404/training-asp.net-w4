using System.ComponentModel.DataAnnotations;

namespace ParcelDeliveryTrackingAsp.Areas.AuthServices.Models
{
    public class RegistrationVM
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? UserName { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Email { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Password { get; set; }

        [StringLength(60, MinimumLength = 8)]
        [Required]
        public string? FirstName { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? LastName { get; set; }

        public string? Role { get; set; }
    }
}
