using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PRMApi.Data;
using PRMApi.Models;
using PRMDataManager.Library.DataAccess;
using PRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IUserData _userData;
        private readonly ILogger<UserController> _logger;
        public UserController(ApplicationDbContext context, UserManager<IdentityUser> usermanager, IUserData userData, ILogger<UserController> logger)
        {
            _context = context;
            _usermanager = usermanager;
            _userData = userData;
            _logger = logger;
        }

        [HttpGet]
        public UserModel GetUserById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            return _userData.GetUserById(userId).First();
        }

        public record UserRegistrationModel(string FirstName, string LastName, string EmailAddress, string Password);
 
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationModel user)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _usermanager.FindByEmailAsync(user.EmailAddress);
                if(existingUser is null)
                {
                    IdentityUser newUser = new() {
                        Email = user.EmailAddress,
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                        UserName = user.EmailAddress
                    };

                    var result = await _usermanager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {

                        try
                        {
                            var addedUser = await _usermanager.FindByEmailAsync(user.EmailAddress);

                            if (addedUser is not null)
                            {
                                UserModel userInfo = new()
                                {
                                    Id = addedUser.Id,
                                    EmailAddress = addedUser.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    CreateDate = DateTime.Now
                                };

                                _userData.SaveUser(userInfo);

                                return Ok();
                            }

                            
                        }
                        catch (Exception ex)
                        {

                            _logger.LogError(ex.Message, null);
                        }

                        
                    }
                }
                else
                {
                    var existingId = existingUser.Id;
                    if (existingId != null)
                    {
                        var existingUserdata = _userData.GetUserById(existingId);
                        if (existingUserdata.Count == 0)
                        {
                            try
                            {
                                UserModel userInfo = new()
                                {
                                    Id = existingId,
                                    EmailAddress = user.EmailAddress,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    CreateDate = DateTime.Now
                                };

                                _userData.SaveUser(userInfo);

                                return Ok();
                            }
                            catch (Exception ex)
                            {

                                _logger.LogError(ex.Message, null);
                            }
                        }
                    }
                    
                }

            }

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

                output.Add(u);
            }
            return output;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _userData.GetUserById(loggedInUserId).First();

            var user = await _usermanager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} added user {User} to role {Role}", 
                loggedInUser.FirstName + " " + loggedInUser.LastName, user.Id, pairing.RoleName);

            await _usermanager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _userData.GetUserById(loggedInUserId).First();


            var user = await _usermanager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} removed user {User} from role {Role}",
                loggedInUser.FirstName + " " + loggedInUser.LastName, user.Id, pairing.RoleName);

            await _usermanager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}

