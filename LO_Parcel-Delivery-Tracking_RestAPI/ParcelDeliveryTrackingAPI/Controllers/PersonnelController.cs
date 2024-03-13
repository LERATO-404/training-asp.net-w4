using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Helpers;
using log4net;
using ParcelDeliveryTrackingAPI.Repositories;
using ParcelDeliveryTrackingAPI.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace ParcelDeliveryTrackingAPI.Controllers
{

    /// <summary>
    /// A summary about PersonnelController class.
    /// </summary>
    /// <remarks>
    /// PersonnelController requires a user to be logged in and have specific role to access the end points
    /// PersonnelController has the following end points:
    /// Get all personnel - required role is Administrator or Manager
    /// Get a personnel by personnel Id - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Get a personnel by personnel first and last name - required role is Administrator
    /// Get all personnel by availability (on Duty or off Duty) - required role is Administrator or Manager
    /// Create a new personnel - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Update an existing personnel - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Delete an existing personnel - required role is Administrator
    /// Using PersonnelRepository
    /// </remarks>


    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PersonnelController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("PersonnelController");

        private readonly ParcelDeliveryTrackingDBContext _parcelContext;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public PersonnelController(ParcelDeliveryTrackingDBContext parcelContext,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext,
            RoleManager<IdentityRole> roleManager)
        {
           _parcelContext = parcelContext;

            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }

       

        // GET: api/Personnel 
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<List<Personnel>>> GetAllPersonnel()
        {
            /*logger.Info("PersonnelController - GET:  api/Personnel");

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");
            bool userAuthorisationManager = await _identityHelper.IsUserInRole(userId, "Manager");
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);


            if (rightRole)
            {
                try
                {
                    PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                    var personnel  = _personnelRepository.GetAllRows();
                    if (personnel == null)
                    {
                        logger.Warn("PersonnelController - GET:  api/Personnel - Not Found / invalid personnel, logged in UserName: " + UserName);
                        return NotFound();
                    }

                    return Ok(personnel);
                }
                catch (Exception ex)
                {
                    logger.Error("PersonnelController - GET:  api/Personnel - Not Found / invalid personnel, logged in UserName: " + UserName + ".  Exception: " + ex);
                    return BadRequest(new { message = "An error occurred while fetching Personnel." });
                }
            }
            else
            {
                logger.Warn($"PersonnelController - GET:  api/Personnel logged in User: {user}");
                return BadRequest(new { message = "Not Authorised." });
            }*/

            try
            {
                PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                var personnel = _personnelRepository.GetAllRows();
                if (personnel == null)
                {
                    //logger.Warn("PersonnelController - GET:  api/Personnel - Not Found / invalid personnel, logged in UserName: " + UserName);
                    return NotFound();
                }

                return Ok(personnel);
            }
            catch (Exception ex)
            {
                //logger.Error("PersonnelController - GET:  api/Personnel - Not Found / invalid personnel, logged in UserName: " + UserName + ".  Exception: " + ex);
                return BadRequest(new { message = "An error occurred while fetching Personnel." });
            }

        }

        // GET: api/Personnel/6
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Personnel>> GetPersonnelById(int id)
        {
            logger.Info($"PersonnelController - GET:  api/Personnel/{id}");

            PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
            var personnel =  _personnelRepository.GetRowById(id);

            if(personnel == null)
            {
                return NotFound(new { message = $"Personnel with ID {id} was not found." });
            }
            return Ok(personnel);
        }

        // GET: api/Personnel/fullname
        [EnableCors("AllowOrigin")]
        [HttpGet("fullname")]
        public async Task<ActionResult<IEnumerable<Personnel>>> GetPersonnelByFullName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            logger.Info($"PersonnelController - GET:  api/Personnel/fullname");
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");
            
            if (userAuthorisationAdmin)
            {
                try
                {
                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    {
                        logger.Error($"PersonnelController - GET:  api/Personnel/fullname - Not Found / invalid personnel - first and last name are required. logged in UserName: {UserName} ");
                        return BadRequest(new { message = "Both first name and last name are required for the search." });
                    }
                    PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                    var personnel =  _personnelRepository.GetPersonnelByFullName(firstName, lastName);

                    if (personnel == null)
                    {
                        return NotFound(new { message = $"No personnel found with the specified full name '{firstName} {lastName}'." });

                    }
                    return Ok(personnel);
                }
                catch (Exception ex)
                {
                    logger.Error($"PersonnelController - GET:  api/Personnel - Not Found / invalid personnel, logged in UserName: {UserName}. Exception:  + {ex}");
                    return StatusCode(500, new { message = "An error occurred while fetching Personnel by full name." });
                }
            }
            else
            {
                logger.Warn($"PersonnelController - GET:  api/Personnel logged in User: {user}");
                return BadRequest(new { message = "Not Authorised." });
            }  
        }

        // GET: api/Personnel/onduty
        [EnableCors("AllowOrigin")]
        [HttpGet("onduty")]
        public async Task<ActionResult<List<Personnel>>> GetPersonnelOnDuty()
        {
            logger.Info($"PersonnelController - GET:  api/Personnel/onduty");

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");
            bool userAuthorisationManager = await _identityHelper.IsUserInRole(userId, "Manager");

            bool rightRole = await _identityHelper.IsSuperUserRole(userId);


            if (rightRole)
            {
                try
                {
                    string availability = "On Duty";
                    PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                    var allPersonnelOnDuty =  _personnelRepository.GetPersonnelByAvailability(availability);
                    return Ok(allPersonnelOnDuty);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred while fetching personnel on duty." });
                }
            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }


        }

        // GET: api/Personnel/offduty
        [EnableCors("AllowOrigin")]
        [HttpGet("offduty")]
        public async Task<ActionResult<List<Personnel>>> GetPersonnelOffDuty()
        {
            /* logger.Info($"PersonnelController - GET:  api/Personnel/offduty");

             string userId = User.Claims.First(c => c.Type == "UserID").Value;
             var user = await _userManager.FindByIdAsync(userId);
             string UserName = user.UserName;

             bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");
             bool userAuthorisationManager = await _identityHelper.IsUserInRole(userId, "Manager");

             bool rightRole = await _identityHelper.IsSuperUserRole(userId);


             if (rightRole)
             {
                 try
                 {
                     string availability = "Off Duty";
                     PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                     var allPersonnelOffDuty =  _personnelRepository.GetPersonnelByAvailability(availability);
                     return Ok(allPersonnelOffDuty);

                 }
                 catch (Exception ex)
                 {
                     return StatusCode(500, new { message = "An error occurred while fetching personnel off duty." });
                 }
             }
             else
             {
                 return BadRequest(new { message = "Not Authorised." });
             }*/
            try
            {
                string availability = "Off Duty";
                PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                var allPersonnelOffDuty = _personnelRepository.GetPersonnelByAvailability(availability);
                return Ok(allPersonnelOffDuty);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching personnel off duty." });
            }
        }

        // POST: api/Personnel
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<ActionResult> PostPersonnel(PersonnelDto personnel)
        {
            try
            {
                PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                return Ok(_personnelRepository.CreateNewPersonnel(personnel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating a new Personnel." });
            }
        }

        // PUT: api/Personnel/2
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonnel(int id, Personnel personnel)
        {
            if (id != personnel.PersonnelId)
            {
                return BadRequest();
            }

            PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
            try
            {

                var (updatePersonnel, message) = _personnelRepository.UpdateRow(personnel);
                return Ok(new { updatePersonnel, message });
            }
            catch (DbUpdateConcurrencyException)
            {
                bool personnelExists = _personnelRepository.PersonnelExists(id);
                    if (!personnelExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to update personnel details. Please check your input and try again." });
            }
            
        }

        // DELETE: api/Personnel/6
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnel(int id)
        {
            PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
            try
            {
                bool deletePersonnel = _personnelRepository.DeleteRow(id);

                if (deletePersonnel == false)
                {
                    return NotFound(new { message = $"Personnel with ID {id} was not found." });
                }

                return Ok(new { message = $"Personnel ID {id} has been successfully deleted." });
            }
            catch (DbUpdateConcurrencyException)
            {
                bool personnelExists = _personnelRepository.PersonnelExists(id);
                if (!personnelExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the personnel. Try again later." });
            }
            /*string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");

            if (userAuthorisationAdmin)
            {
                PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                try
                {
                    bool deletePersonnel =  _personnelRepository.DeleteRow(id);

                    if (deletePersonnel == false)
                    {
                        return NotFound(new { message = $"Personnel with ID {id} was not found." });
                    }

                    return Ok(new { message = $"Personnel ID {id} has been successfully deleted." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool personnelExists = _personnelRepository.PersonnelExists(id);
                    if (!personnelExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred while deleting the personnel. Try again later." });
                }
            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }*/



        }

        
    }
}
