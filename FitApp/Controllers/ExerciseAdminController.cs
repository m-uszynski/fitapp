using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitApp.Models;

namespace FitApp.Controllers
{
    public class ExerciseAdminController : Controller
    {
        // GET: ExerciseAdmin
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
        public ActionResult Add(Exercise model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = new ExerciseAdmin();
                exercise.ExerciseName = model.ExerciseName;
                exercise.VideoLink = model.VideoLink;
                exercise.BurnedCalories = model.BurnedCalories;
                exercise.Description = model.Description;
                db.ExerciseAdmins.Add(exercise);
                db.SaveChanges();
            }
            return RedirectToAction("List", "ExerciseAdmin");
        }

        [Authorize]
        public ActionResult List()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.ExerciseAdmins.ToList();
                return View(exercise);
            }
        }

        [Authorize]
        public ActionResult Delete(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.ExerciseAdmins.Where(m => m.Id == Id).FirstOrDefault();
                db.ExerciseAdmins.Remove(exercise);
                db.SaveChanges();
                return RedirectToAction("List", "ExerciseAdmin");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.ExerciseAdmins.Where(m => m.Id == Id).FirstOrDefault();

                if (exercise != null)
                {
                    ViewBag.ExerciseName = exercise.ExerciseName;
                    ViewBag.VideoLink = exercise.VideoLink;
                    ViewBag.BurnedCalories = exercise.BurnedCalories;
                    ViewBag.Description = exercise.Description;
                }
                else
                {
                    return RedirectToAction("List", "ExerciseAdmin");
                }

            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Exercise model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.ExerciseAdmins.Where(m => m.Id == model.Id).FirstOrDefault();
                if (exercise != null)
                {
                    ViewBag.ExerciseName = exercise.ExerciseName;
                    ViewBag.VideoLink = exercise.VideoLink;
                    ViewBag.BurnedCalories = exercise.BurnedCalories;
                    ViewBag.Description = exercise.Description;
                    exercise.ExerciseName = model.ExerciseName;
                    exercise.VideoLink = model.VideoLink;
                    exercise.BurnedCalories = model.BurnedCalories;
                    exercise.Description = model.Description;
                    db.SaveChanges();
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("List", "ExerciseAdmin");
            }
        }
    }
}