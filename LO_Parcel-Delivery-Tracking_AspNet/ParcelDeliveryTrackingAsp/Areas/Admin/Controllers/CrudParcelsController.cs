using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CrudParcelsController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("CrudParcelsController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public CrudParcelsController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }


        // Get all parcels
        public async Task<IActionResult> Index()
        {
            logger.Info("Fetching all parcels in the Admin area.");
            /* logger.Info("Index - Admin Get all Parcels");

             CurrentUserInfoVM currentUserInfo = GetUserInfo();
             logger.Info("Index - Admin Get all Parcels  - By current User : " + currentUserInfo.UserName + "  UserRole" + currentUserInfo.UserRole);


             bool isAdminUser = UserHelper.IsValidatedAdmin(currentUserInfo.UserToken, currentUserInfo.UserTokenExpiration, currentUserInfo.UserRole);

             if (string.IsNullOrEmpty(currentUserInfo.UserToken) || (!isAdminUser))
             {
                 logger.Info("Index - Admin Get all Parcels - (string.IsNullOrEmpty(currentUserInfo.UserToken) or NOT isAdminUser)");

                 TempData["UserHomePageUrl"] = "/Home/Index";
                 TempData.Keep();


                 return RedirectToAction("Login", "UserManager", new { area = "AuthServices" });
             }*/
            try
            {
                List<ParcelDto> parcels = new List<ParcelDto>();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlParcels = baseUrl + "/api/Parcels";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlParcels);

                if (resp.IsSuccessStatusCode)
                {
                    var results = await resp.Content.ReadAsStringAsync();
                    parcels = JsonConvert.DeserializeObject<List<ParcelDto>>(results);
                    logger.Info($"Successfully retrieved {parcels.Count} parcels.");
                }
                else
                {
                    logger.Warn("Failed to fetch parcels: HTTP status code indicates failure.");
                }
                return View(parcels);
            }
            catch(Exception ex) 
            {
                logger.Error($"An error occurred while fetching parcels: {ex.Message}");
                return View();
            }
            
        }


        // Get a parcel by Id
        public async Task<IActionResult> Details(int id)
        {
            logger.Info("Index - Admin Get a Parcel by Id");
            logger.Info($"Fetching details for a parcel with ID: {id}");
            try
            {
                ParcelDto parcelById = new ParcelDto();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlParcel = baseUrl + $"/api/Parcels/{id}";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlParcel);


                if (resp.IsSuccessStatusCode)
                {
                    var results = resp.Content.ReadAsStringAsync().Result;
                    parcelById = JsonConvert.DeserializeObject<ParcelDto>(results);
                    logger.Info($"Successfully retrieved details for parcel ID {id}.");
                }
                else
                {
                    logger.Warn($"Failed to fetch details for parcel ID {id}: HTTP status code indicates failure.");
                }
                return View(parcelById);
            }
            catch(Exception ex) 
            {
                logger.Error($"An error occurred while fetching details for parcel ID {id}: {ex.Message}");
                return View();
            }
            
        }


        // Get all parcels by parcels status In Transit/Delivered/On Hold
        public async Task<IActionResult> ParcelsByStatus(string status)
        {
            logger.Info("Index - Admin Get all Parcels by Parcel Status");
            logger.Info($"Fetching parcels with status: {status}");
            try
            {
                List<ParcelDto> parcelsByStatus = new List<ParcelDto>();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlParcelsByStatus = baseUrl + $"/api/Parcels/{status}";

                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlParcelsByStatus);

                if (resp.IsSuccessStatusCode)
                {
                    var results = await resp.Content.ReadAsStringAsync();
                    parcelsByStatus = JsonConvert.DeserializeObject<List<ParcelDto>>(results);
                    logger.Info($"Successfully retrieved parcels with status: {status}");
                }
                else
                {
                    logger.Warn($"Failed to fetch parcels with status: {status}. HTTP status code indicates failure.");
                }
                return View(parcelsByStatus);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while fetching parcels with status: {status}. Error: {ex.Message}");
                return View();
            }  
        }


        // Update view 
        public async Task<IActionResult> Update(int id)
        {
            ParcelDto parcelToUpdate = await GetParcelById(id);
            return View(parcelToUpdate);
        }


        //Update request
        [HttpPost]
        public ActionResult Update(ParcelDto parcelToUpdate)
        {
            logger.Info("Index - Admin Update Parcels details");
            logger.Info($"Updating parcel details for Parcel ID: {parcelToUpdate.ParcelId}");
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlParcel = baseUrl + $"/api/Parcels/{parcelToUpdate.ParcelId}";

                var putResponse = _httpClient.PutAsJsonAsync(apiUrlParcel, parcelToUpdate);
                putResponse.Wait();
                var result = putResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully updated parcel details for Parcel ID: {parcelToUpdate.ParcelId}");
                    logger.Info("Update operation completed successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to update parcel with Parcel ID: {parcelToUpdate.ParcelId}. HTTP status code indicates failure.");
                    logger.Warn("Update operation failed.");
                }
                return View();
            }
            catch(Exception ex) 
            {

                logger.Error($"An error occurred while updating parcel: {ex.Message}");
                return View();
            }

        }


        // Delete view
        public async Task<IActionResult> Delete(int id)
        {
            ParcelDto parcelToDelete = await GetParcelById(id);
            return View(parcelToDelete);
        }

        // Delete request
        [HttpPost]
        public ActionResult Delete(ParcelDto parcelToDelete)
        {
            logger.Info("Index - Admin Delete a Parcels");
            logger.Info($"Deleting parcel with Parcel ID: {parcelToDelete.ParcelId}");
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlParcel = baseUrl + $"/api/Parcels/{parcelToDelete.ParcelId}";

                var deleteResponse = _httpClient.DeleteAsync(apiUrlParcel);
                deleteResponse.Wait();
                var result = deleteResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully deleted parcel with Parcel ID: {parcelToDelete.ParcelId}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to delete parcel with Parcel ID: {parcelToDelete.ParcelId}. HTTP status code indicates failure.");
                    logger.Warn("Delete operation failed.");
                }

                return View();
            }
            catch(Exception ex)
            {

                logger.Error($"An error occurred while deleting parcel: {ex.Message}");
                return View();
            }
        }

        //Admin/CrudParcels/TotalParcels
        public async Task<IActionResult> TotalParcels()
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlParcel = baseUrl + $"/api/Parcels";
            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlParcel);

            if (resp.IsSuccessStatusCode)
            {
                var results = await resp.Content.ReadAsStringAsync();
                List<ParcelDto> parcels = JsonConvert.DeserializeObject<List<ParcelDto>>(results);
                int totalParcels = parcels.Count();
                ViewBag.ParcelsCount = totalParcels;
                return View(parcels);
            }
            else
            {
                return Content("Unable to retrieve the total number of parcels.");
            }
        }
        // Get all the parcels for a specific sender

        //Helper Methods
        private async Task<ParcelDto> GetParcelById(int id)
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlParcel = baseUrl + $"/api/Parcels/{id}";

            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlParcel);
            ParcelDto parcel = new ParcelDto();

            if (resp.IsSuccessStatusCode)
            {
                var results = resp.Content.ReadAsStringAsync().Result;
                parcel = JsonConvert.DeserializeObject<ParcelDto>(results);
            }

            return parcel;
        }

        public CurrentUserInfoVM GetUserInfo()
        {
            CurrentUserInfoVM currentUser = new CurrentUserInfoVM();

            //Get data from tempdata
            currentUser.UserHomePageUrl = TempData["UserHomePageUrl"]?.ToString();
            currentUser.UserToken = TempData["UserToken"]?.ToString();
            currentUser.UserTokenExpiration = TempData["UserTokenValidTo"]?.ToString();
            currentUser.FirstName = TempData["FirstName"]?.ToString();
            currentUser.LastName = TempData["LastName"]?.ToString();
            currentUser.UserName = TempData["UserName"]?.ToString();
            currentUser.UserRole = TempData["UserRole"]?.ToString();
            TempData.Keep();

            return currentUser;
        }
    }
}
