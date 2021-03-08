using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PRMDataManager.Library.DataAccess;
using PRMDataManager.Library.Models;
using PRMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: User/Details/5
        [HttpGet]
        public UserModel GetUserById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();


            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var users = userManager.Users.ToList();
            var roles = _context.Roles.ToList();

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                foreach (var r in user.Roles)
                {
                    u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).FirstOrDefault().Name);
                }

                output.Add(u);
            }
            return output;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public void AddRole(UserRolePairModel pairing)
        {

            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            userManager.AddToRole(pairing.UserId, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveRole(UserRolePairModel pairing)
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            userManager.RemoveFromRole(pairing.UserId, pairing.RoleName);
        }
    }
}



