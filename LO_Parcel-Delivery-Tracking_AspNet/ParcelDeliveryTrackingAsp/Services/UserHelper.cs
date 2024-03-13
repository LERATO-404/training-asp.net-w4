using log4net;

namespace ParcelDeliveryTrackingAsp.Services
{
    public class UserHelper
    {
        private static readonly ILog logger = LogManager.GetLogger("UserHelper");

        public static bool IsTokenValid(string? userToken, string? userTokenExpiration)
        {
            DateTime currentDate = DateTime.Now;
            bool validToken = false;

            if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(userTokenExpiration))
            {
                validToken = false;
            }
            else
            {
                var tokenExpirationDate = DateTime.Parse(userTokenExpiration);
                validToken = tokenExpirationDate >= currentDate;
            }

            logger.Info("Checking IsTokenValid currentDate: " + currentDate);
            logger.Info("Checking IsTokenValid userToken: " + userToken);
            logger.Info("Checking IsTokenValid userTokenExpiration: " + userTokenExpiration);
            logger.Info("Checking IsTokenValid validToken: " + validToken);

            return (validToken);
        }

        public static bool IsValidatedAdmin(string? userToken, string? userTokenExpiration, string? userRole)
        {
            bool validToken = IsTokenValid(userToken, userTokenExpiration);
            bool validAdmin = false;

            if (string.IsNullOrEmpty(userRole) || (!validToken))
            {
                validAdmin = false;
            }
            else
            {
                validAdmin = (userRole == "Administrator" && validToken);
            }

            logger.Info("Checking IsValidatedAdmin userRole: " + userRole);
            logger.Info("Checking IsValidatedAdmin userTokenExpiration: " + userTokenExpiration);
            logger.Info("Checking IsValidatedAdmin validToken: " + validToken);
            logger.Info("Checking IsValidatedAdmin validAdmin: " + validAdmin);

            return (validAdmin);
        }


        public static bool IsValidatedTrader(string? userToken, string? userTokenExpiration, string? userRole)
        {
            bool validToken = IsTokenValid(userToken, userTokenExpiration);
            bool validTrader = false;

            if (string.IsNullOrEmpty(userRole) || (!validToken))
            {
                validTrader = false;
            }
            else
            {
                validTrader = (userRole == "Brokerage" && validToken);
            }

            logger.Info("Checking IsValidatedTrader userRole: " + userRole);
            logger.Info("Checking IsValidatedTrader userTokenExpiration: " + userTokenExpiration);
            logger.Info("Checking IsValidatedTrader validToken: " + validToken);
            logger.Info("Checking IsValidatedTrader validTrader: " + validTrader);

            return (validTrader);
        }
    }
}
