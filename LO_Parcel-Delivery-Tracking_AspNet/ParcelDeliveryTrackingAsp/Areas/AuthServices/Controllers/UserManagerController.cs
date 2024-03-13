using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParcelDeliveryTrackingAsp.Areas.AuthServices.Models;
using ParcelDeliveryTrackingAsp.Services;

namespace ParcelDeliveryTrackingAsp.Areas.AuthServices.Controllers
{
    [Area("AuthServices")]
    public class UserManagerController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger("UserManagerController");
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;
        public UserManagerController(IOptions<ClientSettings> ctSettings, HttpClient injectedClient)
        {
            _clientSettings = ctSettings.Value;
            _httpClient = injectedClient;
        }

        public IActionResult Register()
        {
            return View();
        }

        // POST: Register page
        [HttpPost]
        public IActionResult Register(RegistrationVM usrRegData)
        {
            try
            {
                UserRegisteredVM newlyRegUser = new UserRegisteredVM();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlRegister = baseUrl + "/api/ApplicationUser/Register";

                var postResponse = _httpClient.PostAsJsonAsync<RegistrationVM>(apiUrlRegister, usrRegData);
                postResponse.Wait();
                var result = postResponse.Result;

                ViewBag.POSTresultHeader = result.Headers;
                ViewBag.POSTresultStatusCode = result.StatusCode;
                ViewBag.POSTresultRequestMessage = result.RequestMessage;
                ViewBag.POSTresultIsSuccessStatusCode = result.IsSuccessStatusCode;

                if (result.IsSuccessStatusCode)
                {
                    var results = result.Content.ReadAsStringAsync().Result;
                    newlyRegUser = JsonConvert.DeserializeObject<UserRegisteredVM>(results);

                }

                TempData["POSTRegUserUserName"] = newlyRegUser.UserName;
                TempData.Keep();

                // show login page - Once Created
                //return RedirectToAction("Login", "Home", null);
                return RedirectToAction("Index", "Home", new { area = "default" });
            }
            catch (Exception e)
            {
                logger.Error("HttpPost - Registration - UserName: " + usrRegData.UserName + " Exception: " + e);
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Login page
        [HttpPost]
        public IActionResult Login(LoginVM usrLogin)
        {
            logger.Info("HttpPost - Login - UserName: " + usrLogin.UserName);

            try
            {
                CurrentUserVM currentLoggedInUser = new CurrentUserVM();
                string baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlLogin = baseUrl + "/api/ApplicationUser/Login";

                //Set home url for user after successful login
                string userHomePageUrl = baseUrl + "/Home/Index";

                var postResponse = _httpClient.PostAsJsonAsync<LoginVM>(apiUrlLogin, usrLogin);
                postResponse.Wait();
                var result = postResponse.Result;

                //logger.Info("HttpPost - Login: Login page result: " + result);

                ViewBag.POSTresultHeader = result.Headers;
                ViewBag.POSTresultStatusCode = result.StatusCode;
                ViewBag.POSTresultRequestMessage = result.RequestMessage;
                ViewBag.POSTresultIsSuccessStatusCode = result.IsSuccessStatusCode;

                if (result.IsSuccessStatusCode)
                {
                    var results = result.Content.ReadAsStringAsync().Result;
                    currentLoggedInUser = JsonConvert.DeserializeObject<CurrentUserVM>(results);
                }

                //storing data into tempdata  
                TempData["UserHomePageUrl"] = userHomePageUrl;
                TempData["UserToken"] = currentLoggedInUser.Token;
                TempData["UserTokenValidTo"] = currentLoggedInUser.Expiration;
                TempData["FirstName"] = currentLoggedInUser.FirstName;
                TempData["LastName"] = currentLoggedInUser.LastName;
                TempData["UserName"] = currentLoggedInUser.UserName;
                TempData["UserRole"] = currentLoggedInUser.Roles[0];
                TempData.Keep();


                if (result.IsSuccessStatusCode)
                {
                    if (currentLoggedInUser != null && currentLoggedInUser.Roles[0] == "Administrator")
                    {
                        TempData["UserHomePageUrl"] = "/Admin/Home/Index";
                        TempData.Keep();
                        return RedirectToAction("Index", "CrudDeliveries", new { area = "Admin" });
                    }
                    else if (currentLoggedInUser != null && currentLoggedInUser.Roles[0] == "Manager")
                    {
                        TempData["UserHomePageUrl"] = "/Manager/Home/Index";
                        TempData.Keep();
                        return RedirectToAction("Index", "ManagerDeliveries", new { area = "Manager" });
                    }
                    else if (currentLoggedInUser != null && currentLoggedInUser.Roles[0] == "Driver")
                    {
                        TempData["UserHomePageUrl"] = "/Driver/Home/Index";
                        TempData.Keep();
                        return RedirectToAction("Index", "DriverDeliveries", new { area = "Driver" });
                    }
                    else
                    {
                        ////Create Traders Content first  Traders/Brokers/Index
                        TempData["UserHomePageUrl"] = "/Home/Index";
                        TempData.Keep();
                        //return RedirectToAction("Index", "Brokers", new { area = "Traders" });

                        //TempData["UserHomePageUrl"] = "/Home/Index";
                        return RedirectToAction("Index", "Home", new { area = "default" });
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                logger.Error("HttpPost - Login - UserName: " + usrLogin.UserName + " Exception: " + e);

                return View();
            }
        }


        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login", "UserManager", new { area = "AuthServices" });
        }


        public async Task<IActionResult> UserProfile()
        {
            logger.Info("Http GetAsync - UserProfile ");
            try
            {
                //https://localhost:7043/api/UserProfile
                string? baseUrl = _clientSettings.ClientBaseUrl;
                string apiUrlProfile = baseUrl + "/api/UserProfile";
                string userHomePageUrl;

                UserProfileVM currentUserProfile = new UserProfileVM();

                string? _userLoginToken = TempData["UserToken"]?.ToString();
                string? _userLoginTokenValidTo = TempData["UserTokenValidTo"]?.ToString();

                //bool validToken = UserHelper.IsTokenValid(_userLoginToken, _userLoginTokenValidTo);

                //logger.Info("Http GetAsync - UserProfile _userLoginTokenValidTo: " + _userLoginTokenValidTo);
                //logger.Info("Http GetAsync - UserProfile validToken: " + validToken);

                if (string.IsNullOrEmpty(_userLoginToken))// || (!validToken))
                {

                    TempData["UserHomePageUrl"] = "/Home/Index";
                    TempData.Keep();
                    return RedirectToAction("Login", "UserManager", new { area = "AuthServices" });
                }
                else
                {
                    TempData.Keep();
                    HttpClient _client = new HttpClient().AddBearerToken(_userLoginToken);

                    var resp = await _client.GetAsync(apiUrlProfile);

                    logger.Warn("Http GetAsync - UserProfile resp.IsSuccessStatusCode: " + resp.IsSuccessStatusCode);

                    if (resp.IsSuccessStatusCode)
                    {
                        var result = resp.Content.ReadAsStringAsync().Result;
                        currentUserProfile = JsonConvert.DeserializeObject<UserProfileVM>(result);
                    }

                    logger.Info("Http GetAsync - UserProfile currentUserProfile: " + currentUserProfile.UserName);
                    return View(currentUserProfile);
                }
            }
            catch (Exception e)
            {
                logger.Error("Http GetAsync - UserProfile - Exception: " + e.Message);
                return View();
            }
        }

    }
}
