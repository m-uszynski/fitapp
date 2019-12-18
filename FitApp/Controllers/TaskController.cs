using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace FitApp.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Task model,int Id)
        {
            var message = "";
            if(ModelState.IsValid)
            {
                using (FitAppContext db = new FitAppContext())
                {
                    Task task = new Task();
                    task.ChallengeId = Id;
                    task.Description = model.Description;
                    task.RealizationTime = model.RealizationTime;
                    db.Tasks.Add(task);
                    db.SaveChanges();
                }

                return RedirectToAction("List", "Task", new { Id = Id });
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
            using (FitAppContext db = new FitAppContext())
            {
                var tasks = db.Tasks.Where(m => m.ChallengeId == Id).ToList();
                //obsluzyc action link Create new dodajac mu challenge id z url
                ViewBag.ChallengeId = Id;
                return View(tasks);
            }
        }

        [Authorize]
        public ActionResult Realization(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var tasks = db.Tasks.Where(m => m.ChallengeId == Id).ToList();
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();

                List<Task> notDone = new List<Task>();
                foreach(var t in tasks)
                {
                    if(!user.TasksDone.Contains(t))
                    {
                        notDone.Add(t);
                    }
                }

                return View(notDone);
            }

                     
        }

        [Authorize]
        public ActionResult Progress()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();

                List<string> subjects = new List<string>();
                foreach(var t in user.TasksDone)
                {
                    subjects.Add(t.Challanges.Subject);
                }
                ViewBag.subjects = subjects;
                
                return View(user.TasksDone);
            }
               
        }



        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var task = db.Tasks.Where(m => m.Id == Id).FirstOrDefault();
                ViewBag.Description= task.Description;
                ViewBag.RealizationTime= task.RealizationTime.ToShortDateString();
            }
                return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Task model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var task = db.Tasks.Where(m => m.Id == model.Id).FirstOrDefault();
                if(task!=null)
                {
                    ViewBag.Description = model.Description;
                    ViewBag.RealizationTime = model.RealizationTime.ToShortDateString();
                    task.Description = model.Description;
                    task.RealizationTime = model.RealizationTime;
                }
                db.SaveChanges();
            }
            ViewBag.Message = "Dane zostały uaktualnione";
                return View();
        }
    }
}