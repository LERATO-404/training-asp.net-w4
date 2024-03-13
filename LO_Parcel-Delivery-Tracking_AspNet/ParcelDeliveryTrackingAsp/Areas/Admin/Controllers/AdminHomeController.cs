using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {

        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public AdminHomeController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
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
