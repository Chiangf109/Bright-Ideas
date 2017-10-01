using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.Controllers
{
    public class LoginRegController : Controller
    {
        private CSharpContext _context;
    
        public LoginRegController(CSharpContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = new List<string>();
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            TryValidateModel(model);
            ViewBag.errors = ModelState.Values;
    
            if (ModelState.IsValid){
                User ThisUser = _context.users.SingleOrDefault(u => u.Email == model.Email);
                if (ThisUser == null){
                    User NewUser = new User{
                        Name = model.Name,
                        Alias = model.Alias,
                        Email = model.Email,
                        Password = model.Password
                    };
                    _context.users.Add(NewUser);
                    _context.SaveChanges();

                    User InSession = _context.users.SingleOrDefault(user => user.Email == model.Email);
                    HttpContext.Session.SetInt32("UserId", (int)InSession.UserId);
                    ViewBag.InSession = InSession;
                    return RedirectToAction("BrightIdeas", "CSharp");
                } else {
                    TempData["error"] = "Email already in use";
                }
            }
            return View("Index");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string Email, string Password)
        {
            ViewBag.errors = ModelState.Values;

            User ThisUser = _context.users.SingleOrDefault(u => u.Email == Email);
            if (ThisUser != null){
                if (ThisUser.Password == Password){
                    HttpContext.Session.SetInt32("UserId", ThisUser.UserId);     
                    return RedirectToAction("BrightIdeas", "CSharp");
                }
            } else {
                TempData["error"] = "Invalid Email/Password combination";
            }

            return View("Index");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LogReg");
        }
    }
}    