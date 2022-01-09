﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEIIIAssignment.Models;
using BC = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SEIIIAssignment.Controllers
{
    public class AccountController : Controller
    {
        private readonly SEIIIContext _context;
        // GET
        public AccountController(SEIIIContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
          public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles= "Admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            // if (User.Identity.Role)
            // {
            //     
            // }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Role,Password,UserName")] User user)
        {
            var userExists = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if (ModelState.IsValid && userExists == null ){
                user.Password = BC.HashPassword(user.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email,Role,Password")] User user)
        {
            if (id != user.UserId)
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
                    if (!UserExists(user.UserId))
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Login()
        {
           
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Items");
            }
            return View();
        }
      

        [HttpPost]
        public IActionResult Login(string username, string password)
        {

           if (User.Identity.IsAuthenticated) {
               return RedirectToAction("Index", "Items");
            }
            var userData = _context.Users.SingleOrDefault(x => x.UserName == username);
            if (userData == null || !BC.Verify(password, userData.Password))
            {
                return View();
            }

            ClaimsIdentity identity = null;
            bool isAuthenticate = false;
            if (userData.Role =="Admin")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.GivenName,userData.Name)
                   

                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
            }

            if (userData.Role =="Client")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Client")

                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
            }

            if (isAuthenticate)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Name", userData.Name);

                HttpContext.Session.SetString("Role", userData.Role);
                return RedirectToAction("Index", "Items");
            }

            return View("Login");
        }
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");

        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,Name,Email,Password,UserName")] User user)
        {
            var userExists = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if (ModelState.IsValid && userExists == null ){
                user.Password = BC.HashPassword(user.Password);
                user.Role = "Client";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            ViewData["Message"] = "User already exits. Try another username";
            return View(user);
        }

        public RedirectToActionResult AccessDenied()
        {
            TempData["Message"] = "Access Denied. You have been redirected back to the item page";
            return RedirectToAction("Index", "Items");
            
        }
    }
}