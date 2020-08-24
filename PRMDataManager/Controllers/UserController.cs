using Microsoft.AspNet.Identity;
using PRMDataManager.Library.DataAccess;
using PRMDataManager.Library.Models;
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

        // GET: User/Details/5
        public List<UserModel> GetUserById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).ToList();
        }
    }
}
