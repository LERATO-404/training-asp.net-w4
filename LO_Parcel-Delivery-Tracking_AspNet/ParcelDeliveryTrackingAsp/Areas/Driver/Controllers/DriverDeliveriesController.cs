using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Areas.AuthServices.Models;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.Driver.Controllers
{
    [Area("Driver")]
    public class DriverDeliveriesController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("DriverDeliveriesController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public DriverDeliveriesController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }

        // Get all deliveries
        // Driver/DriverDeliveries/Index
        public async Task<IActionResult> Index()
        {
            logger.Info("Fetching all Deliveries for the Driver Index page");
            try
            {
               
                List<Delivery> allDeliveries = new List<Delivery>();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDelivery = baseUrl + "/api/Deliveries";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDelivery);

                if (resp.IsSuccessStatusCode)
                {
                    var results = await resp.Content.ReadAsStringAsync();
                    allDeliveries = JsonConvert.DeserializeObject<List<Delivery>>(results);
                    logger.Info($"Successfully retrieved {allDeliveries.Count} deliveries.");
                }
                else
                {
                    logger.Warn("Failed to fetch deliveries: HTTP status code indicates failure.");
                }
                return View(allDeliveries);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while fetching deliveries: {ex.Message}");
                return View();
            }

            
        }

        
        // Get a delivery by id
        // Admin/CrudDeliveiries/Details/1
        public async Task<IActionResult> MyDeliveries(int id)
        {

            CurrentUserVM currentLoggedInUser = new CurrentUserVM();


            List<Delivery> allMyDeliveries = new List<Delivery>();
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlDelivery = baseUrl + $"/api/Deliveries/{id}/deliveries";
            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDelivery);


            if (resp.IsSuccessStatusCode)
            {
                var results = resp.Content.ReadAsStringAsync().Result;
                allMyDeliveries = JsonConvert.DeserializeObject<List<Delivery>>(results);
            }
            return View(allMyDeliveries);
        }


        public async Task<IActionResult> Update(int id)
        {
            Delivery deliveryToUpdate = await GetDeliveryById(id);
            return View(deliveryToUpdate);
        }


        [HttpPost]
        public ActionResult Update(Delivery delivery)
        {
            logger.Info("Index - Driver Update a Delivery");
            logger.Info($"Updating delivery with Delivery ID: {delivery.DeliveryId}");
            try
            {

                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDelivery = baseUrl + $"/api/Deliveries/{delivery.DeliveryId}";

                var putResponse = _httpClient.PutAsJsonAsync(apiUrlDelivery, delivery);
                putResponse.Wait();
                var result = putResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully updated delivery details for Delivery ID: {delivery.DeliveryId}");
                    logger.Info("Update operation completed successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to update delivery with Delivery ID: {delivery.DeliveryId}. HTTP status code indicates failure.");
                    logger.Warn("Update operation failed.");
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while updating delivery: {ex.Message}");
                return View();
            }
        }



        //Helper Methods
        private async Task<Delivery> GetDeliveryById(int id)
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlDelivery = baseUrl + $"/api/Deliveries/{id}";

            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDelivery);
            Delivery delivery = new Delivery();

            if (resp.IsSuccessStatusCode)
            {
                var results = resp.Content.ReadAsStringAsync().Result;
                delivery = JsonConvert.DeserializeObject<Delivery>(results);
            }

            return delivery;
        }
    }
}
