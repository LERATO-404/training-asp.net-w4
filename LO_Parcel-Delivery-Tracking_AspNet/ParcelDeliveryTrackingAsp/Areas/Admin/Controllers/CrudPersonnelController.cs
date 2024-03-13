using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Models;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CrudPersonnelController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("CrudPersonnelController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;

        public CrudPersonnelController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }


        // Get all personnel
        public async Task<IActionResult> Index()
        {
            logger.Info("Fetching all Personnel for the Admin Index page");
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

        // Get a personnel by Id
        public async Task<IActionResult> Details(int id)
        {
            logger.Info("Index - Admin Get a personnel by Id");
            logger.Info($"Fetching details for a personnel using ID: {id}");
            try
            {
                Personnel personnelById = new Personnel();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlPersonnel = baseUrl + $"/api/Personnel/{id}";
                HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlPersonnel);


                if (resp.IsSuccessStatusCode)
                {
                    var results = resp.Content.ReadAsStringAsync().Result;
                    personnelById = JsonConvert.DeserializeObject<Personnel>(results);
                    logger.Info($"Successfully retrieved details for personnel ID {id}.");
                }
                else
                {
                    logger.Warn($"Failed to fetch details for personnel ID {id}: HTTP status code indicates failure.");
                }
                return View(personnelById);

            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while fetching details for personnel ID {id}: {ex.Message}");
                return View();
            }

            
        }


        // create a new personnel
        // put a personnel using Id
        public async Task<IActionResult> Update(int id)
        {
            Personnel personnelToUpdate = await GetPersonnelById(id);
            return View(personnelToUpdate);
        }

        [HttpPost]
        public ActionResult Update(Personnel personnel)
        {
            logger.Info("Index - Admin Update a Personnel");
            logger.Info($"Deleting Personnel with Personnel ID: {personnel.PersonnelId}");
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlPersonnel = baseUrl + $"/api/Personnel/{personnel.PersonnelId}";

                var putResponse = _httpClient.PutAsJsonAsync(apiUrlPersonnel, personnel);
                putResponse.Wait();
                var result = putResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully updated personnel details for Personnle ID: {personnel.PersonnelId}");
                    logger.Info("Update operation completed successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to update personnle with Personnel ID: {personnel.PersonnelId}. HTTP status code indicates failure.");
                    logger.Warn("Update operation failed.");
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while updating personnel: {ex.Message}");
                return View();
            }
        }

        // delete a personnel
        public async Task<IActionResult> Delete(int id)
        {
            Personnel personnelToDelete = await GetPersonnelById(id);
            return View(personnelToDelete);
        }


        [HttpPost]
        public ActionResult Delete(Personnel personnel)
        {
            logger.Info("Index - Admin Delete a Personnel");
            logger.Info($"Deleting personnle with Personnel ID: {personnel.PersonnelId}");
            try
            {
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlPersonnel = baseUrl + $"/api/Personnel/{personnel.PersonnelId}";

                var deleteResponse = _httpClient.DeleteAsync(apiUrlPersonnel);
                deleteResponse.Wait();
                var result = deleteResponse.Result;

                if (result.IsSuccessStatusCode)
                {
                    logger.Info($"Successfully deleted personnel with Personnel ID: {personnel.PersonnelId}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    logger.Warn($"Failed to delete personnel with Personnel ID: {personnel.PersonnelId}. HTTP status code indicates failure.");
                    logger.Warn("Delete operation failed.");
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while deleting personnel: {ex.Message}");
                return View();
            }
        }



        // get a personnel using full name
        // get all the personnnel on duty
        // get all the personnel off duty



        //Admin/CrudPersonnel/TotalPersonnel
        public async Task<IActionResult> TotalPersonnel()
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlPersonnel = baseUrl + $"/api/Personnel";
            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlPersonnel);

            if (resp.IsSuccessStatusCode)
            {
                var results = await resp.Content.ReadAsStringAsync();
                List<Personnel> personnel = JsonConvert.DeserializeObject<List<Personnel>>(results);
                int totalPersonnel = personnel.Count();

                return View(totalPersonnel);
            }
            else
            {
                return Content("Unable to retrive the total number of personnel.");
            }
        }

        //Helper Methods
        private async Task<Personnel> GetPersonnelById(int id)
        {
            string baseUrl = _clientSettings.ClientBaseUrl;
            string apiUrlPersonnel = baseUrl + $"/api/Personnel/{id}";

            HttpResponseMessage resp = await _httpClient.GetAsync(apiUrlPersonnel);
            Personnel personnel = new Personnel();

            if (resp.IsSuccessStatusCode)
            {
                var results = resp.Content.ReadAsStringAsync().Result;
                personnel = JsonConvert.DeserializeObject<Personnel>(results);
            }

            return personnel;
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
