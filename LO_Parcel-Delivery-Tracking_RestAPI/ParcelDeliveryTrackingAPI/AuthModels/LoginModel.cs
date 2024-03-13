﻿using System.ComponentModel.DataAnnotations;

namespace ParcelDeliveryTrackingAPI.AuthModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
