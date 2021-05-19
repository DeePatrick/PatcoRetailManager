using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRMApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PRMApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> usermanager)
        {
            _logger = logger;
            _rolemanager = rolemanager;
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {
            return View();
        }


        //temp for database
        public async Task<IActionResult> Privacy()
        {
            string[] roles = { "Admin", "Manager", "Cashier" };

            foreach (var role in roles)
            {
                var roleExist = await _rolemanager.RoleExistsAsync(role);

                if (roleExist == false)
                {
                    await _rolemanager.CreateAsync(new IdentityRole(role));
                }
            }



            var user = await _usermanager.FindByEmailAsync("ginika@gmail.com");

            if (user.EmailConfirmed)
            {
                await _usermanager.AddToRoleAsync(user, "Admin");
                await _usermanager.AddToRoleAsync(user, "Cashier");
            }

            var user2 = await _usermanager.FindByEmailAsync("okudopato@gmail.com");

            if (user2.EmailConfirmed)
            {
                await _usermanager.AddToRoleAsync(user2, "Admin");
                await _usermanager.AddToRoleAsync(user2, "Cashier");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
