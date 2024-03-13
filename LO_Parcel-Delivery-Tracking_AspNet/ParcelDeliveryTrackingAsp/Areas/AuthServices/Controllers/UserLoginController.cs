using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.AuthServices.Controllers
{
    [Area("AuthServices")]
    public class UserLoginController : Controller
    {
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public UserLoginController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}
