using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class ChallengeController : Controller
    {
        // GET: Challenge
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Challenge model,int GroupId)
        {
            var message = "";
            if(ModelState.IsValid)
            {
                using (FitAppContext db = new FitAppContext())
                {
                    var group = db.Groups.Where(m => m.Id == GroupId).FirstOrDefault();
                    Challenge challenge = new Challenge();
                    challenge.Subject = model.Subject;
                    challenge.EndDate = model.EndDate;
                    challenge.GroupId = GroupId;

                    group.Challenges.Add(challenge);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "Challenge", new { Id = GroupId });
            }
            else
            {
                message = "Uzupełnij dane";
            }
            ViewBag.Message = message;
            return View(model);

        }

        [Authorize]
        public ActionResult List(int Id)
        {
            ViewBag.GroupId = Id;
            using (FitAppContext db = new FitAppContext())
            {
                var challenge = db.Challenges.Where(m => m.GroupId == Id).ToList();

                var group = db.Groups.Where(m => m.Id == Id).FirstOrDefault();
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                ViewBag.AdminId = group.AdminId;
                ViewBag.UserId = user.Id;

                return View(challenge);
            }
        }

        [Authorize]
        public ActionResult End(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var challenge = db.Challenges.Where(m => m.Id == Id).FirstOrDefault();
                var groupId = challenge.GroupId;

                var tasks = challenge.Tasks.ToList();

                foreach(var t in tasks)
                {
                    foreach(var u in t.UsersWhoDid)
                    {
                        TaskRepository.TaskEdit(t, u.Id, challenge.Subject);
                    }
                    
                }

                db.Challenges.Remove(challenge);
                db.SaveChanges();

                return RedirectToAction("List", "Challenge",new { Id=groupId});
            }
                
        }
    }
}