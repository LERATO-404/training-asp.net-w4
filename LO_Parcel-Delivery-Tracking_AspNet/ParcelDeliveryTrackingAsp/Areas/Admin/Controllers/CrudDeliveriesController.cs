using log4net;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;
using System.Diagnostics;
using System.Net;
using System.Text;


namespace ParcelDeliveryTrackingAsp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CrudDeliveriesController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("CrudDeliveriesController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public CrudDeliveriesController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }


        // Get all deliveries
        // Admin/CrudDeliveries/Index
        public async Task<IActionResult> Index()
        {
            
            logger.Info("Fetching all deliveries for the Admin Index page");

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
            catch(Exception ex)
            {
                logger.Error($"An error occurred while fetching deliveries: {ex.Message}");
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
            logger.Info("Index - Admin Create a new Delivery");
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




        // Get a delivery by using the Id
        // Admin/CrudDeliveiries/Details/1
        public async Task<IActionResult> Details(int id)
        {
            logger.Info("Index - Admin Get a Parcel by Id");
            logger.Info($"Fetching details for a parcel with ID: {id}");

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



        // Patch a delivery using the id
        // Patch a delivery by using delivery Id
        public async Task<IActionResult> Update(int id)
        {
            Delivery deliveryToUpdate = await GetDeliveryById(id);
            return View(deliveryToUpdate);
        }

        [HttpPost]
        public ActionResult Update(Delivery delivery)
        {
            logger.Info("Index - Admin Update a Delivery");
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





        // Delete a delivery
        public async Task<IActionResult> Delete(int id)
        {
            Delivery deliveryToDelete = await GetDeliveryById(id);

            return View(deliveryToDelete);
        }

        [HttpPost]
        public ActionResult Delete(Delivery delivery)
        {
            logger.Info("Index - Admin Delete a Delivery");
            logger.Info($"Deleting delivery with Delivery ID: {delivery.DeliveryId}");
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDeliveries = baseUrl + $"/api/Deliveries/{delivery.DeliveryId}";

                var deleteResponse = _httpClient.DeleteAsync(apiUrlDeliveries);
                deleteResponse.Wait();
                var result = deleteResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully deleted delivery with Delivery ID: {delivery.DeliveryId}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to delete delivery with Delivery ID: {delivery.DeliveryId}. HTTP status code indicates failure.");
                    logger.Warn("Delete operation failed.");
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while deleting a delivery: {ex.Message}");
                return View();
            }
        }

        // Get deliveries using the status
        // Admin/CrudDeliveries/DeliveryByStatus/Status/{status}
        public async Task<IActionResult> DeliveriesByStatus(string status)
        {
            logger.Info("Index - Admin Get all Deliveries by Delivery Status");
            logger.Info($"Fetching deliveries with status: {status}");
            try
            {
                List<Delivery> deliveriesByStatus = new List<Delivery>();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlDeliveriesByStatus = baseUrl + $"/api/Deliveries/Status/{status}";

                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDeliveriesByStatus);

                if (resp.IsSuccessStatusCode)
                {
                    var results = await resp.Content.ReadAsStringAsync();
                    deliveriesByStatus = JsonConvert.DeserializeObject<List<Delivery>>(results);
                    logger.Info($"Successfully retrieved deliveries with status: {status}");
                }
                else
                {
                    logger.Warn($"Failed to fetch deliveries with status: {status}. HTTP status code indicates failure.");
                }
                return View(deliveriesByStatus);

            }
            catch(Exception ex)
            {
                logger.Error($"An error occurred while fetching deliveries with status: {status}. Error: {ex.Message}");
                return View();
            }


            
        }


        //Admin/CrudDeliveries/TotalDeliveries
        public async Task<IActionResult> TotalDeliveries()
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlDelivery = baseUrl + $"/api/Deliveries";
            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlDelivery);

            if(resp.IsSuccessStatusCode)
            {
                var results = await resp.Content.ReadAsStringAsync();
                List<Delivery> deliveries = JsonConvert.DeserializeObject<List<Delivery>>(results);
                int totalDeliveries = deliveries.Count();

                return View(totalDeliveries);
            }
            else
            {
                return Content("Unable to retrive the total number of deliveries.");
            }
        }

        // Get deliveries for a personnel using personnel Id


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
