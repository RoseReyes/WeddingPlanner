using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using wedding.Models;

namespace wedding.Controllers
{
    public class HomeController : Controller
    {
        private weddingContext _context;

         public HomeController(weddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult register(RegisterViewModel registerVM)
        { 
            if(ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Email = registerVM.Email,
                    Password = registerVM.Password
                };

                //Hashed Password
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                //Save to DB
                 _context.Add(user);
                 _context.SaveChanges();
                 HttpContext.Session.SetInt32("active_user", user.UserId);
                 int? id = HttpContext.Session.GetInt32("active_user");
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        

        [HttpPost]
        [Route("login")]
        public IActionResult login(User userlogin)
        {   
             if(ModelState.IsValid)
             {
                    List<User> ReturnedValues = _context.Users.Where(s => s.Email.Equals(userlogin.Email)).ToList();
                    foreach (var users in ReturnedValues)
                    {                    
                        if(users.Email != null && userlogin.Email != null)
                        {
                            var Hasher = new PasswordHasher<User>();
                            // Pass the user object, the hashed password, and the PasswordToCheck
                            if(0 != Hasher.VerifyHashedPassword(users, users.Password, userlogin.Password))
                            {
                                //get userid and set it to session
                                HttpContext.Session.SetInt32("active_user", users.UserId);
                                int? id = HttpContext.Session.GetInt32("active_user");
                                System.Console.WriteLine("I am here");
                                return RedirectToAction("Dashboard");
                            }  
                        }
                        TempData["error"] = "Invalid email or password. Check your email or register!";
                        return View("Index");
                    }
            };
            TempData["error"] = "Invalid email or password. Check your email or register!";
            return View("Index");
        }

        [HttpGet]
        [Route("RedirectWedding")]
        public IActionResult RedirectWedding()
        {   
            int? id = HttpContext.Session.GetInt32("active_user");

            if(id == null)
            {
                return RedirectToAction("/");
            }
            System.Console.WriteLine("Iam here");
            return View("PlanWedding");
        }

        [HttpPost]
        [Route("CreateWedding")]
        public IActionResult CreateWedding(PlanWeddingViewModel planwed)
        {   
            if(ModelState.IsValid)
            {
                if (planwed.Date < DateTime.Today) 
                {
                    TempData["error"] = "Date should be future-date";
                    return View("PlanWedding");
                }
                int? id = HttpContext.Session.GetInt32("active_user");
                int userid = Convert.ToInt32(id);
                
                WeddingPlanner newwedding = new WeddingPlanner
                {
                    WedderOne = planwed.WedderOne,
                    WedderTwo = planwed.WedderTwo,
                    Date = planwed.Date,
                    WedAddress = planwed.WedAddress,
                    UserId = userid
                };
                //Save to DB Weddings
                _context.Add(newwedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        return View("PlanWedding");
        }

        [HttpGet]
        [Route("Guest/{id?}")]
        public IActionResult Guest(int id) {
            List<WeddingPlanner> ReturnedValues = _context.Weddings.Where(w => w.WeddingPlannerId == id).Include(u => u.guests).ThenInclude( m => m.User).ToList();
            ViewBag.Results = ReturnedValues;
            return View("Guest");
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard(){
            int? id = HttpContext.Session.GetInt32("active_user");
            ViewBag.Creator = id;

            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var Weddings = _context.Weddings.Include(w => w.user).Include(u => u.guests).ThenInclude( m => m.User).ToList();
            ViewBag.Results = Weddings;
            return View("Dashboard");
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult rsvp(int weddingid)
        {            
            UserWedding addguest = new UserWedding 
            {
                UserId = (int) HttpContext.Session.GetInt32("active_user"),
                WeddingId = weddingid
            };
            System.Console.WriteLine(addguest);
            //Save to DB Guests
            _context.Add(addguest);
            _context.SaveChanges();
            System.Console.WriteLine("Iam here rsvp");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("unrsvp")]
        public IActionResult unrsvp(int weddingid)
        {   
            UserWedding removeguest = _context.Guests.FirstOrDefault(a => a.WeddingId == weddingid);
            _context.Guests.Remove(removeguest);
            _context.SaveChanges();
            System.Console.WriteLine("Iam here");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult delete(int weddingid)
        {
             WeddingPlanner deletewedding = _context.Weddings.FirstOrDefault(a => a.WeddingPlannerId == weddingid);
            _context.Weddings.Remove(deletewedding);
            _context.SaveChanges();
            System.Console.WriteLine("Iam here delete");
            return RedirectToAction("Dashboard");
        }
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
