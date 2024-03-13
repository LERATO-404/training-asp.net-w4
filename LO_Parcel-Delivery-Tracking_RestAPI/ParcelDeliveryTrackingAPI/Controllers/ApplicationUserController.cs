using log4net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParcelDeliveryTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("ApplicationUserController");

        private readonly ParcelDeliveryTrackingDBContext _parcelContext;

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings, ParcelDeliveryTrackingDBContext parcelContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _parcelContext = parcelContext;
        }

        [HttpPost]
        [Route("Register")]
        [EnableCors("AllowOrigin")]
        // Post : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Register");
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Register model:" + model.LastName);

            var personnel = new PersonnelDto()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.Email,
                UserName = model.UserName,

            };
            var appUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            

            if (model.Role == null)
            {
                // change to User
                model.Role = "Driver";
            }

            try
            {
                if(model.Role == "Manager" || model.Role == "Driver")
                {
                    PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                    _personnelRepository.CreateNewPersonnel(personnel);
                }

               /* if (model.Role == "User")
                {
                    //Parcel participant
                }*/
                
                var result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {
                    var userResult = await _userManager.AddToRoleAsync(appUser, model.Role);
                }
                return Ok(new { username = model.UserName, message = $"User {appUser.FirstName} {appUser.LastName} Created Successfully." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to create user. Try again later." });
            }
        }


        [HttpPost]
        [Route("Login")]
        [EnableCors("AllowOrigin")]
        // Post : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login");
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login model.UserName:" + model.UserName);


            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var claim = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserID", user.Id.ToString())
                    };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SigningKey));
                string tmpKeyIssuer = _appSettings.JWT_Site_URL;
                int expiryInMinutes = Convert.ToInt32(_appSettings.ExpiryInMinutes);


                var usrToken = new JwtSecurityToken(
                    claims: claim,
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(usrToken),
                    expiration = usrToken.ValidTo,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    roles = await _userManager.GetRolesAsync(user)
                });

            }
            else
            {
                logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login - BadRequest:");

                return BadRequest(new { message = "Username or password not found." });
            }

        }
    }
}
