using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RoutingDemo.Data;
using RoutingDemo.Models;

namespace RoutingDemo.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly UserContext _context;



        public HomeController(ILogger<HomeController> logger, UserContext context) {
            _logger = logger;
            _context = context;
        }

        //-----------------------------------------


        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Students() {
            return this.RedirectToAction("Index", "Students");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // --------------------------------------------------------

        // GET on Login method is in HomeController -> Index
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Password,Email")] User user) {
            var users = await _context.User.ToListAsync();

            var userDataIEnumerable = from u in users
                           where (u.Email == user.Email)
                           select u;
            var userData = userDataIEnumerable.ToList()[0];
            //var userData = await _context.User.FindAsync(user.Email);

            if (userData == null) {
                return NotFound();
            }

            user.Password = Hasher.GetHashString(user.Password, userData?.FirstName);
            if (user.Password == userData.Password) {

                if (HttpContext.Session.GetString("LoggedUser") == null) {
                    HttpContext.Session.SetString("LoggedUser", JsonConvert.SerializeObject(userData));
                }

                return RedirectToAction("LoginSuccess");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult LoginSuccess() {
            return View();
        }

        public IActionResult NoPermission() {
            return View();
        }
    }
}
