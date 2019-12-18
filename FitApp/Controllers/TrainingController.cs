using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class TrainingController : Controller
    {
        // GET: Training
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(Training model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
                if(user!=null)
                {
                    if (ModelState.IsValid)
                    {
                        model.UserId = user.Id;
                        model.BurnedCalories = 0;
                        model.ExerciseCount = 0;
                        db.Trainings.Add(model);
                        db.SaveChanges();
                    }
                    else
                    {
                        return View(model);
                    }
                    return RedirectToAction("List", "Training");
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult List()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
                ViewBag.uType = user.UserType;
                if (user != null)
                {
                    var trainings = db.Trainings.Where(u => u.UserId.Equals(user.Id)).ToList();
                    return View(trainings);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult Delete(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var tr = db.Trainings.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                if(tr.ExerciseCount>0)
                {
                    var exercises = db.Exercises.Where(u => u.TrainingId == tr.Id).FirstOrDefault();
                    db.Exercises.Remove(exercises);
                }

                db.Trainings.Remove(tr);
                db.SaveChanges();
                return RedirectToAction("List", "Training");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var tr = db.Trainings.Where(m => m.Id == Id).FirstOrDefault();
                if(tr!=null)
                {
                    ViewBag.TrainingName = tr.TrainingName;
                    ViewBag.TrainingTime = tr.TrainingTime.ToShortDateString();
                }
                else
                {
                    return RedirectToAction("List", "Training");
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Training model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var tr = db.Trainings.Where(m => m.Id == model.Id).FirstOrDefault();
                if(tr!=null)
                {
                    ViewBag.TrainingName = tr.TrainingName;
                    ViewBag.TrainingTime = tr.TrainingTime.ToShortDateString();
                    tr.TrainingName = model.TrainingName;
                    tr.TrainingTime = model.TrainingTime;
                    db.SaveChanges();
                }
                else
                {
                    return View(model);
                }
            }
            return RedirectToAction("List", "Training");
        }

        [Authorize]
        public ActionResult UserTrainings()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var users = db.Users.ToList();
                return View(users);
            }
        }

        [Authorize]
        public ActionResult UserTrainingList(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Id == Id).FirstOrDefault();
                ViewBag.userLogin = user.Login;
                var trainings = db.Trainings.Where(m => m.UserId == Id).ToList();
                return View(trainings);
            }
        }

        [Authorize]
        public ActionResult UserExerciseList(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercises = db.Exercises.Where(m => m.TrainingId == Id).ToList();
                var tra = db.Trainings.Where(m => m.Id == Id).FirstOrDefault();
                ViewBag.userId = tra.UserId;
                ViewBag.exname = tra.TrainingName;
                

                return View(exercises);
            }
        }
    }
}