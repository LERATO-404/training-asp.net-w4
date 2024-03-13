using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;
using System.Net.Http;

namespace ParcelDeliveryTrackingAsp.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManagerPersonnelController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("ManagerPersonnelController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public ManagerPersonnelController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }

        // Get all personnel
        public async Task<IActionResult> Index()
        {
            logger.Info("Fetching all Personnel for the Manager Index page");
            try
            {
                List<Personnel> personnel = new List<Personnel>();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlPersonnel = baseUrl + "/api/Personnel";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlPersonnel);

                if (resp.IsSuccessStatusCode)
                {
                    var results = await resp.Content.ReadAsStringAsync();
                    personnel = JsonConvert.DeserializeObject<List<Personnel>>(results);
                    logger.Info($"Successfully retrieved {personnel.Count} personnel.");
                }
                else
                {
                    logger.Warn("Failed to fetch personnel: HTTP status code indicates failure.");
                }
                return View(personnel);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while fetching personnel: {ex.Message}");
                return View();
            }
            
        }
    }
}
