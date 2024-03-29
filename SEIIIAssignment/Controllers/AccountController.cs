﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SEIIIAssignment.Models;
using BC = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using MimeKit;

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
              ViewBag.UnApproved = await _context.Users.Where(u => u.ApprovalStatus == 0).ToListAsync();
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
       
            return View();
        }
        [Authorize(Roles = "Admin,Client")]
        public IActionResult Dashboard()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            var id = Int32.Parse(userId);
            var user =  _context.Users
                .FirstOrDefault(m => m.UserId == id);
            ViewBag.BoughtItems = _context.Items.Where(i => i.BoughtbyId == id).ToList();
            ViewBag.SoldItems = _context.Items.Where(i => i.PostedbyId == id).ToList();
            ViewBag.ArchivesByUser = _context.Items.Where(i => i.PostedbyId == id && i.ArchiveStatus==1).ToList();
            
            ViewBag.Archives = _context.Items.Where(i=> i.ArchiveStatus==1).ToList();
            
            return View(user);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Role,Password,UserName")] User user)
        {
            var userExists = _context.Users.FirstOrDefault(x => x.UserName == user.UserName || x.Email==user.Email);
            if (ModelState.IsValid && userExists == null ){
                user.Password = BC.HashPassword(user.Password);
                user.ApprovalStatus = 1;
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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

          
            var bids = _context.Bids.Where(x => x.BidderId == user.UserId).ToList();
            foreach (var bid in bids)
            {
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
            }     
            var items = _context.Items.Where(x => x.Postedby.UserId == user.UserId).ToList();
            foreach (var item in items)
            {
             
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int? id)
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

            user.ApprovalStatus = 1;
            _context.Update(user);
            await _context.SaveChangesAsync();
               //The code for the email has been commented out as and can be used by putting in credentials and allowing less secure apps in google accounts
            // var message = new MimeMessage();
            // message.From.Add(new MailboxAddress("Mr. Max Fotheby", ""));
            // message.To.Add(new MailboxAddress(user.Name, user.Email));
            // message.Subject = "Thank you for being a part of us";
            // message.Body = new TextPart("plain")
            // {
            //     Text = "Dear " + user.Name +
            //            ",\n\n\nWe are pleased to have you as a client for our system .\n\nMay I also take this opportunity again to thank you for using Fotherby’s auction house, as we seek to provide you with the best time here.\n\nYours Sincerely,\n\nMr M Fotherby"
            // };
            // using (var client = new SmtpClient())
            // {
            //     client.Connect("smtp.gmail.com", 587, false);
            //     client.Authenticate("","");
            //     client.Send(message);
            //     client.Disconnect(true);
            //         
            // }
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
            if (userData == null || !BC.Verify(password, userData.Password)||userData.ApprovalStatus==0)
            {
                ViewData["Message"] = "Invalid login credentials";
                return View();
            }
//Void Geeks. 2019. Role Based Authorization in ASP.NET Core Application. [online] Available at: <http://www.voidgeeks.com/tutorial/Role-Based-Authorization-in-ASPNET-Core-Application/9> [Accessed 5 January 2022].
            ClaimsIdentity identity = null;
            bool isAuthenticate = false;
            if (userData.Role =="Admin")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username), 
                    new Claim(ClaimTypes.Sid, userData.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.GivenName,userData.Name),new Claim(ClaimTypes.Email,userData.Email)
                   

                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
            }

            if (userData.Role =="Client")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Sid, userData.UserId.ToString()),
                    new Claim(ClaimTypes.GivenName,userData.Name),
                    new Claim(ClaimTypes.Role, "Client"),
                    new Claim(ClaimTypes.GivenName,userData.Name),new Claim(ClaimTypes.Email,userData.Email)
                    
                 

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
            var userExists = _context.Users.FirstOrDefault(x => x.UserName == user.UserName || x.Email==user.Email);
            if (ModelState.IsValid && userExists == null)
            {
                user.ApprovalStatus = 0;
                user.Password = BC.HashPassword(user.Password);
                user.Role = "Client";
                _context.Add(user);
                await _context.SaveChangesAsync();
              
                return RedirectToAction("Login");
            }

            ViewData["Message"] = "User already exits. Try another username or email";
                return View(user);
            }
        

        public RedirectToActionResult AccessDenied()
        {
            TempData["Message"] = "Access Denied. You have been redirected back to the item page";
            return RedirectToAction("Index", "Items");
            
        }
    }
}