using Microsoft.AspNetCore.Mvc;
using RoutingDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace RoutingDemo.Controllers {
    //  the data is's provided by controller to the view template
    public class StudentsController : Controller {

        private List<Student> students = new List<Student>() {
                new Student(1, "John", "Kowalski", 35),
                new Student(2, "Adam", "Smith", 19),
                new Student(3, "Anna", "Adams", 22)
            };

        public void Handle() { }

        // GET: /Students/
        // An HTTP endpoint is a targetable URL in the web application, such as https://localhost:5001/HelloWorld
        // It combines the protocol used: HTTPS, the network location of the web server (including the TCP port): localhost:5001 and the target URI HelloWorld.

        // URL routing logic used by MVC:
        // /[Controller]/[ActionName]/[Parameters]

        public IActionResult Index() {
            return View(students);
        }

        // GET: /Students/Add/
        private void Add() { }

        // GET: /Students/Edit/
        private void Edit(int id) { }

        // GET: /Students/Delete/
        private void Delete(int id) { }
    }
}
