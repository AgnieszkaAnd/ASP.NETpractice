using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoutingDemo.Models;

namespace RoutingDemo.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        private List<Student> students = new List<Student>() {
                new Student(1, "John", "Kowalski", 35),
                new Student(2, "Adam", "Smith", 19),
                new Student(3, "Anna", "Adams", 22)
            };


        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Students() {
            //List<Student> students = new List<Student>() {
            //    new Student(1, "John", "Kowalski", 35),
            //    new Student(2, "Adam", "Smith", 19),
            //    new Student(3, "Anna", "Adams", 22)
            //};
            //ViewData["students"] = students;

            //ViewData["student1"] = students[0];
            //ViewData["student2"] = students[1];
            //ViewData["student3"] = students[2];

            return View(students);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
