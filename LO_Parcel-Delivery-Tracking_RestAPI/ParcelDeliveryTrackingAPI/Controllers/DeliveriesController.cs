using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Helpers;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;

namespace ParcelDeliveryTrackingAPI.Controllers
{

    /// <summary>
    /// A summary about DeliveriesController class.
    /// </summary>
    /// <remarks>
    /// DeliveriesController requires a user to be logged in and have specific role to access the end points
    /// DeliveriesController has the following end points:
    /// Get all deliveries - required role is Administrator or Manager
    /// Get a delivery with the Delivery id - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Get all deliveries with the specified delivery status (Complete, In Progress, or Scheduled) - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Get all deliveries for the specified personnel id - Authenticated user role(s) (Administrator or Manager)
    /// Update delivery status and/or Personnel Id - Authenticated user role(s) (Administrator, Manager, or Driver)
    /// Using DeliveryRepository
    /// </remarks>


    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DeliveriesController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("DeliveriesController");

        private readonly ParcelDeliveryTrackingDBContext _parcelContext;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DeliveriesController(ParcelDeliveryTrackingDBContext context,
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
        
       
        // GET: api/Deliveries 
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<List<Delivery>>> GetAllDeliveries()
        {
            logger.Info("DeliveriesController - GET: api/Deliveries");

            /*  string userId = User.Claims.First(c => c.Type == "UserID").Value;
              var user = await _userManager.FindByIdAsync(userId);
              string UserName = user.UserName;

              bool rightRole = await _identityHelper.IsSuperUserRole(userId);


              if (rightRole)
              {
                  try
                  {
                      DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                      var deliveries =  _deliveryRepository.GetAllRows();
                      return Ok(deliveries);
                  }
                  catch (Exception ex)
                  {
                      return StatusCode(500, new { message = "An error occurred while fetching Deliveries." });
                  }
              }
              else
              {
                  return BadRequest(new { message = "Not Authorised." });
              }   */

            try
            {
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveries = _deliveryRepository.GetAllRows();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Deliveries." });
            }
        }

        // GET: api/Deliveries/4
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDeliveryById(int id)
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/{id}");

            DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
            var deliveryById = await Task.Run(() => _deliveryRepository.GetRowById(id));

            if(deliveryById == null)
            {
                return NotFound(new { message = $"Delivery with ID {id} was not found." });
            }

            return Ok(deliveryById);
        }

        // GET: api/Deliveries/InProgress
        [EnableCors("AllowOrigin")]
        [HttpGet("InProgress")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAllDeliveriesInProgress()
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/InProgress");
            try
            {
                string status = "In Progress";
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryByStatus = await Task.Run(() => _deliveryRepository.GetAllDeliveriesByStatus(status));
                return Ok(deliveryByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching deliveries in progress." });
            }
        }

        // GET: api/Deliveries/Completed
        [EnableCors("AllowOrigin")]
        [HttpGet("Completed")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAllDeliveriesCompleted()
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/Completed");
            try
            {
                string status = "Completed";
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryByStatus = await Task.Run(() => _deliveryRepository.GetAllDeliveriesByStatus(status));
                return Ok(deliveryByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching deliveries completed." });
            }
        }

        // GET: api/Deliveries/Scheduled
        [EnableCors("AllowOrigin")]
        [HttpGet("Scheduled")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAllDeliveriesScheduled()
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/Scheduled");
            try
            {
                string status = "Scheduled";
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryByStatus = await Task.Run(() => _deliveryRepository.GetAllDeliveriesByStatus(status));
                return Ok(deliveryByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching deliveries scheduled." });
            }
        }


        // GET: api/Deliveries/{status}
        [EnableCors("AllowOrigin")]
        [HttpGet("Status/{status}")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAllDeliveriesByStatus(string status)
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/Status/{status}");
            try
            {
                string sts = status;
                //string status = "Scheduled";
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryByStatus = await Task.Run(() => _deliveryRepository.GetAllDeliveriesByStatus(status));
                return Ok(deliveryByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while fetching deliveries with status {status}." });
            }
        }

        // GET: api/Deliveries/4/deliveries
        [EnableCors("AllowOrigin")]
        [HttpGet("{personnelId}/deliveries")]
        public async Task<ActionResult<List<DeliveryDto>>> GetDeliveriesByPersonnel(int personnelId)
        {
            logger.Info($"DeliveriesController - GET: api/Deliveries/{personnelId}/deliveries");
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool rightRole = await _identityHelper.IsSuperUserRole(userId);


            if (rightRole)
            {
                try
                {
                    PersonnelRepository _personnelRepository = new PersonnelRepository(_parcelContext);
                    DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);

                    var personnel = _personnelRepository.PersonnelExists(personnelId);

                    if (!personnel)
                    {
                        return NotFound(new { message = $"Personnel with ID {personnelId} was not found." });
                    }

                    var personnelDeliveries = _deliveryRepository.GetDeliveriesForPersonnel(personnelId);

                    if(personnelDeliveries == null)
                    {
                        return BadRequest(new { message = "Unable to get deliveries for personnel. Please check your input and try again." });
                    }

                    return Ok(personnelDeliveries);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"An error occurred while fetching deliveries for personnel ID {personnelId}." });
                }
            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }

      /*  // PATCH: api/Deliveries/2
        [EnableCors("AllowOrigin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, DeliveryStatusDto deliveryDto)
        {
            logger.Info($"DeliveriesController - PATCH: api/Deliveries/{id}");
            try
            {
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryUpdateStatus = _deliveryRepository.UpdateDeliveryStatus(id, deliveryDto);

                if (deliveryUpdateStatus == null)
                {
                    return NotFound(new { message = $"Delivery with ID {id} was not found. Update failed." });
                }

                return Ok(new { message = $"Delivery status for ID {id} has been successfully updated to {deliveryUpdateStatus.DeliveryStatus}" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to update delivery details. Please check your input and try again." });
            }
        }*/
        

        // PUT: api/Deliveries/3
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveryStatusPut(int id, DeliveryStatusDto deliveryDto)
        {
            logger.Info($"DeliveriesController - PUT: api/Deliveries/{id}");
            try
            {
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var deliveryUpdateStatus = _deliveryRepository.UpdateDeliveryStatus(id, deliveryDto);
                if (deliveryUpdateStatus == null)
                {
                    return NotFound(new { message = $"Delivery with ID {id} was not found. Update failed." });
                }

                return Ok(new { message = $"Delivery status for ID {id} has been successfully updated to {deliveryUpdateStatus.DeliveryStatus}" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to update parcel details. Please check your input and try again." });
            }

        }
      

        // POST: api/Deliveries
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<ActionResult<Delivery>> PostDelivery(DeliveryCreateDto deliveryDto)
        {
            logger.Info($"DeliveriesController - POST:  api/Deliveries");
            try
            {
                DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
                var newDelivery = _deliveryRepository.CreateNewDelivery(deliveryDto);

                return Ok(new { newDelivery });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating a new Delivery. Try again later." });
            }
        }




        // DELETE: api/Delivery/4
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            logger.Info($"DeliveryController - DELETE:  api/Deliveries/{id}");

            DeliveryRepository _deliveryRepository = new DeliveryRepository(_parcelContext);
            var deliveryToDelete = _deliveryRepository.DeleteRow(id);

            if (deliveryToDelete == false)
            {
                return NotFound(new { message = $"Delivery with ID {id} was not found." });
            }

            return Ok(new { message = $"Delivery ID {id} has been successfully deleted." });
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
