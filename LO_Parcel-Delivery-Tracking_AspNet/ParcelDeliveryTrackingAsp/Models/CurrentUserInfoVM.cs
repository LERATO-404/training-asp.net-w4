namespace ParcelDeliveryTrackingAsp.Models
{
    public class CurrentUserInfoVM
    {
        public string? UserHomePageUrl { get; set; }
        public string? UserToken { get; set; }
        public string? UserTokenExpiration { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
    }
}
