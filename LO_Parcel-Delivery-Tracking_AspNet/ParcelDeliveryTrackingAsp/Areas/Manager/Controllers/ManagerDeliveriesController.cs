using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManagerDeliveriesController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("ManagerDeliveriesController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public ManagerDeliveriesController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }

        // Get all deliveries
        // Manager/ManagerDeliveries/Index
        public async Task<IActionResult> Index()
        {
            logger.Info("Fetching all deliveries for the Manager Index page");
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
        // Manager/ManagerDeliveiries/Details/1
        public async Task<IActionResult> Details(int id)
        {
            logger.Info("Index - Manager Get a Delivery by Id");
            logger.Info($"Fetching details for a delivery with ID: {id}");
            try
            {
               
                Delivery deliveryById = new Delivery();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDelivery = baseUrl + $"/api/Deliveries/{id}";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDelivery);


                if (resp.IsSuccessStatusCode)
                {
                    var results = resp.Content.ReadAsStringAsync().Result;
                    deliveryById = JsonConvert.DeserializeObject<Delivery>(results);
                    logger.Info($"Successfully retrieved details for delivery ID {id}.");
                }
                else
                {
                    logger.Warn($"Failed to fetch details for delivery ID {id}: HTTP status code indicates failure.");
                }
                return View(deliveryById);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while fetching details for delivery ID {id}: {ex.Message}");
                return View();
            }


           
        }



        // Create/Post a new delivery
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Delivery delivery)
        {
            logger.Info("Index - Manager Create a new Delivery");
            
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDelivery = baseUrl + "/api/Deliveries";
                var postResponse = _httpClient.PostAsJsonAsync<Delivery>(apiUrlDelivery, delivery);
                postResponse.Wait();

                var result = postResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    
                    logger.Info("Create operation completed successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to create a delivery. HTTP status code indicates failure.");
                    logger.Warn("Create operation failed.");
                }

                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while creating a delivery: {ex.Message}");
                return View();
            }
        }


        public async Task<IActionResult> Update(int id)
        {
            Delivery deliveryToUpdate = await GetDeliveryById(id);
            return View(deliveryToUpdate);
        }

        [HttpPost]
        public ActionResult Update(Delivery delivery)
        {
            logger.Info("Index - Manager Update a Delivery");
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
