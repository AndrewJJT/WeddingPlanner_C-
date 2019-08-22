using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context){
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(User newUser){
            if (ModelState.IsValid){
                
                if (dbContext.users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                //Hash the password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                //Add new user to database
                dbContext.Add(newUser);
                HttpContext.Session.SetString("Status","LogIn");
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");

                }
            }
            else{
                return View("Index");
            }
        }

        public IActionResult ProcessLogin(LoginUser loginUser){
            if (ModelState.IsValid)
            {
            var userFoundInDb = dbContext.users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
            
            // check if email exist....
            if (userFoundInDb == null){
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return RedirectToAction("Login");
            }
            
            // then check if password match....
            var hasher = new PasswordHasher<LoginUser>();

            var result = hasher.VerifyHashedPassword(loginUser, userFoundInDb.Password, loginUser.LoginPassword);

            if(result == 0){
                ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                return RedirectToAction("Login");
            }
            
            //when both email and password check out, get session and login in 
            HttpContext.Session.SetString("Status","LogIn");
            HttpContext.Session.SetInt32("UserId", userFoundInDb.UserId);
            return RedirectToAction("Dashboard");
            }

            else{
                return View("Index");
            }

        }
        
        [HttpGet("DashBoard")]
        public IActionResult Dashboard(){
            if(HttpContext.Session.GetInt32("Status") != null){

                int? currentUserId = HttpContext.Session.GetInt32("UserId");
                int currentLoginId = (int) currentUserId;
                
                ViewBag.UserId = currentLoginId;

                List<Wedding> allweddings = dbContext.weddings.Include(w => w.Guests).ThenInclude(w => w.User).ToList();

                return View(allweddings);
            }
            else{
                return RedirectToAction("Index");
            }
        }

        [HttpGet("wedding/{id}")]
        public IActionResult Detail(int id){

            Wedding selectedWedding = dbContext.weddings.Include(w => w.Guests).ThenInclude(w => w.User).FirstOrDefault(w => w.WeddingId == id);

            return View(selectedWedding);

        }

        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [HttpGet("delete/wedding/{id}")]
        public IActionResult DeleteWedding(int id){

            Wedding weddingToDelete = dbContext.weddings.FirstOrDefault(w => w.WeddingId == id);

            dbContext.weddings.Remove(weddingToDelete);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("UnRSVP/{id}")]
        public IActionResult UnRSVP(int id){

            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            int currentLoginId = (int) currentUserId;
                
            ViewBag.UserId = currentLoginId;
            
            //get all attendances from attandances table (need ?)
            List<Attendance> allattendance = dbContext.attendences.ToList();

            //get the wedding where the current user want to Un rsvp
            Wedding selectedWedding = dbContext.weddings.FirstOrDefault(w => w.WeddingId == id);
            
            //find the attendance column in selectedwedding that is belong to the current user (w/currentUser's Id)
            Attendance userAttendanceForWed = selectedWedding.Guests.FirstOrDefault(u => u.UserId == currentLoginId);
            
            // remove the row/attendance from all attendance
            dbContext.attendences.Remove(userAttendanceForWed);

            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("RSVP/{id}")]
        public IActionResult RSVP(int id){

            
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            int currentLoginId = (int) currentUserId;

            Attendance wedCurrentUserToAttend = new Attendance {UserId = currentLoginId, WeddingId = id};

            dbContext.attendences.Add(wedCurrentUserToAttend);

            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("Create")]
        public IActionResult Create(){
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            int currentLoginId = (int) currentUserId;
            ViewBag.UserId = currentLoginId;
            return View();
        }

        [HttpPost("ProcessCreateWedding")]
        public IActionResult ProcessCreate(Wedding newWedding){
            if (ModelState.IsValid){
                
            dbContext.weddings.Add(newWedding);
            dbContext.SaveChanges();

            return RedirectToAction("DashBoard");
            }
            return View("Create");
        }
    }
}
