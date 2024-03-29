﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoutingDemo.Models;

namespace RoutingDemo.Data
{
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Password,Email")] User user) {

            if (ModelState.IsValid) {
                // The MailAddress class uses a BNF parser to validate the address in full accordance with RFC822.
                bool isEmailValid = false;
                try {
                    MailAddress address = new MailAddress(user.Email);
                    isEmailValid = (address.Address == user.Email);
                } catch (FormatException) {
                    // address is invalid
                    if (isEmailValid) {
                        user.Password = Hasher.GetHashString(user.Password, user.FirstName);

                        //user.Password - to zahashować; zahashowane hasło to będzie string
                        // guid też jest stringiem, ale podzbiorem stringów
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("RegisterSuccess");
                    }
                }
                ModelState.AddModelError("Email", "invalid email");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult RegisterSuccess() {
            return View();
        }

        // GET on Login method is in HomeController -> Index
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Password,Email")] User user) {
            var userData = await _context.User.FindAsync(user.Email);

            if (userData == null) {
                return NotFound();
            }

            user.Password = Hasher.GetHashString(user.Password, userData?.FirstName);
            if (user.Password == userData.Password) {
                return LoginSuccess();
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult LoginSuccess() {
            return View();
        }

        public IActionResult Logout() {
            HttpContext.Session.Remove("LoggedUser");
               
            return RedirectToAction("Index", "Home");
    }

        // GET: Users
        public async Task<IActionResult> Index() {
            if (HttpContext.Session.GetString("LoggedUser") != null) {
                var loggedUser = HttpContext.Session.GetString("LoggedUser");
                User user = JsonConvert.DeserializeObject<User>(loggedUser);

                if (user.FirstName == "admin") {
                    return View(await _context.User.ToListAsync());
                }
            }
            return RedirectToAction("NoPermission", "Home");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Password,Email,IsEmailVerified,ActivationCode,RegisterDate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Password,Email,IsEmailVerified,ActivationCode,RegisterDate")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

        public IActionResult Error() {
            return View();
        }
    }
}
