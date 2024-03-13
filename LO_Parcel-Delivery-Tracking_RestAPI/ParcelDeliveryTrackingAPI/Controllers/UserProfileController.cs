using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Dto;
using ParcelDeliveryTrackingAPI.Helpers;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;

namespace ParcelDeliveryTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {

        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUser> roleManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserProfileController(UserManager<ApplicationUser> userManager, AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;
            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        // Get : /api/UserProfile
        public async Task<Object> Get()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);

            List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserName,
                userRoles
            };
        }


        [EnableCors("AllowOrigin")]
        [HttpPatch("{username}")]
        // Patch : /api/UserProfile/{username}
        public async Task<IActionResult> UpdateUserRole(string username, UserRoleDto userUpdateRoleDto)
        {
            
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var userloggedIn = await _userManager.FindByIdAsync(userId);
                string UserName = userloggedIn.UserName;

                bool userAuthorisationAdmin = await _identityHelper.IsUserInRole(userId, "Administrator");

                if (userAuthorisationAdmin)
                {
                    var user = await _userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        return NotFound(new { message = $"User with username {username} was not found. Update failed." });
                    }

                    if (!await _roleManager.RoleExistsAsync(userUpdateRoleDto.Role))
                    {
                        return BadRequest(new { message = $"Role {userUpdateRoleDto.Role} does not exist." });
                    }

                    // Get the current roles for the user
                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Remove the user from all current roles
                    await _userManager.RemoveFromRolesAsync(user, userRoles);

                    await _userManager.AddToRoleAsync(user, userUpdateRoleDto.Role);
                    await _authContext.SaveChangesAsync();
                    return Ok(new { message = $"User role for username {username} has been successfully updated to {userUpdateRoleDto.Role}" });
                }
                else
                {
                    return BadRequest(new { message = "Not Authorised." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to update delivery details. Please check your input and try again."+ex });
            }

        }
    }
}
