using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Helpers;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;

namespace ParcelDeliveryTrackingAPI.Controllers
{
    /// <summary>
    /// A summary about ParcelsController class.
    /// </summary>
    /// <remarks>
    /// ParcelsController requires a user to be logged in and have specific role to access the end points
    /// ParcelsController has the following end points:
    /// Get all parcels - required role is Administrator 
    /// Get a parcel by parcel Id - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Get all parcels by parcel status (Delivered, In Transit, or On Hold) - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Get all parcels for a specific sender Id - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Create a new parcel -  required role is Administrator or Manager 
    /// Update an existing parcel -  Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Delete an existing parcel -  required role is Administrator 
    /// Using ParcelRepository
    /// </remarks>

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ParcelsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("ParcelsController");

        private readonly ParcelDeliveryTrackingDBContext _parcelContext;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ParcelsController(ParcelDeliveryTrackingDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext,
            RoleManager<IdentityRole> roleManager)
        {
            _parcelContext = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }

        // GET: api/Parcels
        //[EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<List<ParcelDto>>> GetAllParcels()
        {
            logger.Info("ParcelsController - GET:  api/Parcels");

            /*string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");

            if(userAuthorisationAdmin)
            {
                try
                {
                    
                    ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                    var allParcels = _parcelRepository.GetAllRows();
                    return Ok(allParcels);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred while fetching Parcels." });
                }
            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }*/

            try
            {

                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var allParcels = _parcelRepository.GetAllRows();
                return Ok(allParcels);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Parcels." });
            }

        }

        // GET: api/Parcels/2
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ParcelDto>> GetParcelById(int id)
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/{id}");
            try
            {
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcel = _parcelRepository.GetRowById(id);
                if (parcel == null)
                {
                    return NotFound(new { message = $"Parcel with ID {id} was not found." });
                }

                return Ok(parcel);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = $"Unable to get Parcel with ID {id}. Try again later." });
            }
        }

        // GET: api/Parcels/Delivered
        [EnableCors("AllowOrigin")]
        [HttpGet("Delivered")]
        public async Task<ActionResult<List<ParcelDto>>> GetAllParcelByStatus()
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/Delivered");
            try
            {
                string status = "Delivered";
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcels = _parcelRepository.GetAllParcelByStatus(status);
                if (parcels == null)
                {
                    return NotFound(new { message = $"Parcels with status {status} were not found." });
                }

                return Ok(parcels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching parcels delivered. Try again later." });
            }
        }

        // GET: api/Parcels/InTransit
        [EnableCors("AllowOrigin")]
        [HttpGet("InTransit")]
        public async Task<ActionResult<List<ParcelDto>>> GetAllParcelInTransit()
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/InTransit");
            try
            {
                string status = "In Transit";
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcels = _parcelRepository.GetAllParcelByStatus(status);
                if (parcels == null)
                {
                    return NotFound(new { message = $"Parcels with status {status} were not found." });
                }

                return Ok(parcels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching parcels in transit. Try again later." });
            }
        }

        // GET: api/Parcels/OnHold
        [EnableCors("AllowOrigin")]
        [HttpGet("OnHold")]
        public async Task<ActionResult<IEnumerable<ParcelDto>>> GetAllParcelOnHold()
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/OnHold");
            try
            {
                string status = "On Hold";
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcels = _parcelRepository.GetAllParcelByStatus(status);
                if (parcels == null)
                {
                    return NotFound(new { message = $"Parcels with status {status} were not found." });
                }

                return Ok(parcels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching parcels on hold. Try again later." });
            }
        }

        // GET: api/Parcels/{status}
        [EnableCors("AllowOrigin")]
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<ParcelDto>>> GetParcelByStatus(string status)
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/{status}");
            try
            {
                
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcels = _parcelRepository.GetAllParcelByStatus(status);
                if (parcels == null)
                {
                    return NotFound(new { message = $"Parcels with status {status} were not found." });
                }

                return Ok(parcels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while fetching parcels with status {status}. Try again later." });
            }
        }



        // GET: api/Parcels/3/parcels
        [EnableCors("AllowOrigin")]
        [HttpGet("{senderId}/parcels")]
        public async Task<ActionResult<ParcelDto>> GetSenderParcels(int senderId)
        {
            logger.Info($"ParcelsController - GET:  api/Parcels/{senderId}/parcels");
            try
            {
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcels = _parcelRepository.GetSenderParcels(senderId);

                if (parcels == null)
                {
                    return NotFound(new { message = $"Sender with ID {senderId} was not found." });
                }

                return Ok(parcels);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Unable to get parcel for Sender with ID {senderId}. Try again later." });
            }
        }

        // POST: api/Parcels
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<ActionResult<Parcel>> PostParcel(ParcelCreateDto parcelDto)
        {
            logger.Info($"ParcelsController - POST:  api/Parcels");
            try
            {
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var newParcel = _parcelRepository.CreateNewParcel(parcelDto);

                return Ok(new { newParcel });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating a new Parcel. Try again later." });
            }
        }

        // PUT: api/Parcels/3
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParcelStatus(int id, ParcelDto parcel)
        {
            logger.Info($"ParcelsController - PUT:  api/Parcels/{id}");
            try
            {
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcelToUpdate = _parcelRepository.UpdateParcelInformation(id, parcel);
                if (parcelToUpdate == null)
                {
                    return NotFound(new { message = $"Parcel with ID {id} was not found. Update failed." });
                }

                return Ok(new { message = $"Parcel status for Parcel ID {id} has been successfully updated to {parcelToUpdate.ParcelStatus}" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to update parcel details. Please check your input and try again." });
            }

        }

        // DELETE: api/Parcels/4
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcel(int id)
        {
            logger.Info($"ParcelsController - DELETE:  api/Parcels/{id}");

            ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
            var parcelToDelete = _parcelRepository.DeleteRow(id);

            if (parcelToDelete == false)
            {
                return NotFound(new { message = $"Parcel with ID {id} was not found." });
            }

            return Ok(new { message = $"Parcel ID {id} has been successfully deleted." });
            /*string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");

            if(userAuthorisationAdmin)
            {
                ParcelRepository _parcelRepository = new ParcelRepository(_parcelContext);
                var parcelToDelete = _parcelRepository.DeleteRow(id);

                if (parcelToDelete == false)
                {
                    return NotFound(new { message = $"Parcel with ID {id} was not found." });
                }

                return Ok(new { message = $"Parcel ID {id} has been successfully deleted." });
            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }*/

        }
    }
}
