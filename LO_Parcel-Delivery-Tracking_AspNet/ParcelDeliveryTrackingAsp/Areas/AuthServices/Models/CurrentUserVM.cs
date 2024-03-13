namespace ParcelDeliveryTrackingAsp.Areas.AuthServices.Models
{
    public class CurrentUserVM
    {
        public string Token { get; set; }
        public string Expiration { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
