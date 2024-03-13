namespace ParcelDeliveryTrackingAPI.AuthModels
{
    public class ApplicationSettings
    {
        public string? JWT_Secret { get; set; }
        public string? JwtCookieName { get; set; }
        public string? SigningKey { get; set; }
        public string? ExpiryInMinutes { get; set; }
        public string? JWT_Site_URL { get; set; }
        public string? Client_URL { get; set; }
    }
}
