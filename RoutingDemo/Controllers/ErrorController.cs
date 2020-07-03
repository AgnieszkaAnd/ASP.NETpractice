using Microsoft.AspNetCore.Mvc;
using RoutingDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace RoutingDemo.Controllers {
    public class ErrorController : Controller {

        public IActionResult Index() {
            return View();
        }


        public IActionResult Error() {
            return View();
        }
    }
}
