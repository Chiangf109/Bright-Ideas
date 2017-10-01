using System.Collections.Generic;
using System.Linq;
using CSharp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Controllers
{
    public class CSharpController : Controller
    {
        private CSharpContext _context;
    
        public CSharpController(CSharpContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        [Route("BrightIdeas")]
        public IActionResult BrightIdeas()
        {
            ViewBag.InSession = new List<string>();
            ViewBag.AllIdeas = new List<string>();
            ViewBag.AllThinkers = new List<string>();

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be signed in to view this page";
                return RedirectToAction("Index", "LogReg");
            }
            
            User InSession = _context.users.SingleOrDefault(u => u.UserId == SessionId);
            ViewBag.InSession = InSession;

            List<Idea> AllIdeas = _context.ideas.Include(i => i.Likes).OrderByDescending(i => i.Likes.Count).ToList();
            List<User> AllThinkers = new List<User>();
            foreach (var id in AllIdeas){
                User Thinker = _context.users.SingleOrDefault(u => u.UserId == id.UserId);
                AllThinkers.Add(Thinker);
            }

            ViewBag.AllThinkers = AllThinkers;
            ViewBag.AllIdeas = AllIdeas;

            List<Like> AllLikes = _context.likes.ToList();

            return View("BrightIdeas");
        }

        [HttpPost]
        [Route("AddIdea")]
        public IActionResult AddIdea(string IdeaText)
        {
            // ViewBag.SessionId = new List<string>();

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            // ViewBag.SessionId = (int)SessionId;

            if (IdeaText == null){
                TempData["ideaerror"] = "That's not much of an idea! Think a little harder.";
            } else {
                Idea NewIdea = new Idea{
                    UserId = (int)SessionId,
                    IdeaText = IdeaText
                };
                _context.ideas.Add(NewIdea);
                _context.SaveChanges();
            }

            return RedirectToAction("BrightIdeas");
        }

        [HttpGet]
        [Route("Delete/{IdeaId}")]
        public IActionResult Delete(int IdeaId)
        {
            Idea ThisIdea = _context.ideas.Where(i => i.IdeaId == IdeaId).Include(i => i.Likes).SingleOrDefault();
            _context.ideas.Remove(ThisIdea);
            _context.SaveChanges();
            
            return RedirectToAction("BrightIdeas");
        }

        [HttpGet]
        [Route("AddLike/{IdeaId}")]
        public IActionResult AddLike(int IdeaId)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User ThisUser = _context.users.Where(u => u.UserId == SessionId).SingleOrDefault();
            Idea ThisIdea = _context.ideas.Where(i => i.IdeaId == IdeaId).SingleOrDefault();
            Like NewLike = new Like{
                User = ThisUser,
                Idea = ThisIdea
            };
            _context.Add(NewLike);
            _context.SaveChanges();

            
            return RedirectToAction("BrightIdeas");
        }

        [HttpGet]
        [Route("UserProfile/{UserId}")]
        public IActionResult UserProfile(int UserId)
        {
            ViewBag.ThisUser = new List<string>();

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be logged in to view this page";
                return RedirectToAction("Index", "LogReg");
            }

            User ThisUser = _context.users.Where(u => u.UserId == UserId).Include(u => u.Ideas).Include(u => u.Likes).SingleOrDefault();
            ViewBag.ThisUser = ThisUser;

            return View("UserProfile");
        }

        [HttpGet]
        [Route("LikeStatus/{IdeaId}")]
        public IActionResult LikeStatus(int IdeaId)
        {
            ViewBag.ThisIdea = new List<string>();
            ViewBag.Thinker = new List<string>();

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be logged in to view this page";
                return RedirectToAction("Index", "LogReg");
            }

            Idea ThisIdea = _context.ideas.Where(i => i.IdeaId == IdeaId).Include(i => i.Likes).ThenInclude(u => u.User).SingleOrDefault();
            ViewBag.ThisIdea = ThisIdea;

            User Thinker = _context.users.SingleOrDefault(u => u.UserId == ThisIdea.UserId);
            ViewBag.Thinker = Thinker;
            return View("LikeStatus");
        }
    }
}