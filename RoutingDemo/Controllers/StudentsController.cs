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
    //  the data is's provided by controller to the view template
    public class StudentsController : Controller {
        //public readonly ISession _students;
        public StudentsController() {
            //if (HttpContext.Session.GetString("Students") == null) {
            //    HttpContext.Session.SetString("Students", JsonConvert.SerializeObject(students));
            //}
        }
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

            // TODO
            // opakować wnętrze do metody (linie 45-50) i ją wywołać w Index-ie + dodać unit test
            // HttpContext.Session - użyć MOCKOWANIA        (fakes - coś innego, do generowania danych)
            // mock tworzony np. w startupie - wykonywany przed unit testami
            // Unit testy - szczególnie przy większych projektach (długi czas rozwijania + życia) - spr wynik: nie konkretną implementację; spr corner cases

            if (HttpContext.Session.GetString("Students") == null) {
                HttpContext.Session.SetString("Students", JsonConvert.SerializeObject(students));
            }

            
            if (HttpContext.Session.GetString("LoggedUser") != null) {
                var loggedUser = HttpContext.Session.GetString("LoggedUser");
                User user = JsonConvert.DeserializeObject<User>(loggedUser);

                if (user.FirstName == "admin") {
                    var value = HttpContext.Session.GetString("Students");
                    List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);

                    return View(studentsInIndex);
                }
            }
            return RedirectToAction("NoPermission", "Home");
        }

        // GET: /Students/Add/
        [HttpGet]
        public IActionResult Add() {
            return View();
        }

        // POST: /Students/Add/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([Bind("ID","FirstName","LastName","Age")] Student student) {
            // TODO: można metodą GetString opakować w metodę - zgodnie z zasadą DRY
            // może dodać ją do klasy helper
            var value = HttpContext.Session.GetString("Students"); // w większym projekcie może to być pobierane z zewn serwisu i dodany w DI

            List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);

            int newID = studentsInIndex.Count()+1;
            Student newStudent = new Student(newID, student.FirstName, student.LastName, student.Age);

            studentsInIndex.Add(newStudent);
            
            HttpContext.Session.SetString("Students", JsonConvert.SerializeObject(studentsInIndex));

            return this.RedirectToAction("Index", "Students");
        }


        // GET: /Students/Edit/
        [HttpGet]
        public IActionResult Edit(int id) {
            if (id == null) return BadRequest();
            var value = HttpContext.Session.GetString("Students");
            List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);
            Student s;
            try { s = studentsInIndex[id - 1]; } catch (ArgumentOutOfRangeException) { return this.RedirectToAction("Error"); }
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost]
        public IActionResult Edit(int id, Student student) {
            // if (ModelState.IsValid) {} else
            var value = HttpContext.Session.GetString("Students");
            List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);

            studentsInIndex[id-1] = new Student(id, student.FirstName, student.LastName, student.Age);
            HttpContext.Session.SetString("Students", JsonConvert.SerializeObject(studentsInIndex));

            return this.RedirectToAction("Index");
        }

        // GET: /Students/Delete/
        public IActionResult Delete(int id) {
            if (id == null) return BadRequest();
            var value = HttpContext.Session.GetString("Students");
            List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);
            Student s;
            try { s = studentsInIndex[id - 1]; } catch (ArgumentOutOfRangeException) { return this.RedirectToAction("Error"); }
            //Student s = studentsInIndex[id - 1];
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost]
        public IActionResult Delete(int id, Student student) {
            var value = HttpContext.Session.GetString("Students");
            List<Student> studentsInIndex = value == null ? null : JsonConvert.DeserializeObject<List<Student>>(value);

            studentsInIndex.RemoveAt(id - 1);
            HttpContext.Session.SetString("Students", JsonConvert.SerializeObject(studentsInIndex));

            return this.RedirectToAction("Index");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error() {
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Error() {
            return View();
        }
    }
}
