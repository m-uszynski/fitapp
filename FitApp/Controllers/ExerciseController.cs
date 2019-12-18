using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class ExerciseController : Controller
    {
        // GET: Exercise
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
        public ActionResult Add(Exercise model, int TrainingId)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var training = db.Trainings.Where(m => m.Id == TrainingId).FirstOrDefault();
                Exercise exercise = new Exercise();
                exercise.ExerciseName = model.ExerciseName;
                exercise.VideoLink = model.VideoLink;
                exercise.BurnedCalories = model.BurnedCalories;
                exercise.Description = model.Description;
                exercise.TrainingId = TrainingId;
                training.BurnedCalories += model.BurnedCalories;
                training.ExerciseCount += 1;
                training.Exercises.Add(exercise);
                db.SaveChanges();
            }
            return RedirectToAction("List", "Exercise", new { Id = TrainingId });
        }

        [Authorize]
        public ActionResult List(int Id)
        {
            ViewBag.TrainingId = Id;
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.Exercises.Where(m => m.TrainingId == Id).ToList();
                return View(exercise);
            }
        }

        [Authorize]
        public ActionResult Delete(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {         
                var exercise = db.Exercises.Where(m => m.Id == Id).FirstOrDefault();
                var trainingId = exercise.TrainingId;
                var training = db.Trainings.Where(m => m.Id == trainingId).FirstOrDefault();
                training.BurnedCalories -= exercise.BurnedCalories;
                training.ExerciseCount -= 1;
                db.Exercises.Remove(exercise);
                db.SaveChanges();
                return RedirectToAction("List", "Exercise", new { Id = trainingId });
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var exercise = db.Exercises.Where(m => m.Id == Id).FirstOrDefault();
                
                if(exercise!=null)
                {
                    ViewBag.ExerciseName = exercise.ExerciseName;
                    ViewBag.VideoLink = exercise.VideoLink;
                    ViewBag.BurnedCalories = exercise.BurnedCalories;
                    ViewBag.Description = exercise.Description;
                }
                else
                {
                    return RedirectToAction("List", "Exercise");
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
                var exercise = db.Exercises.Where(m => m.Id == model.Id).FirstOrDefault();
                var caloriesNow = exercise.BurnedCalories;
                var trainingId = exercise.TrainingId;
                if (exercise!=null)
                {
                    ViewBag.ExerciseName = exercise.ExerciseName;
                    ViewBag.VideoLink = exercise.VideoLink;
                    ViewBag.BurnedCalories = exercise.BurnedCalories;
                    ViewBag.Description = exercise.Description;
                    exercise.ExerciseName = model.ExerciseName;
                    exercise.VideoLink = model.VideoLink;
                    exercise.BurnedCalories = model.BurnedCalories;
                    exercise.Description = model.Description;
                    var diff =  model.BurnedCalories - caloriesNow;;
                    var training = db.Trainings.Where(m => m.Id == trainingId).FirstOrDefault();
                    training.BurnedCalories += diff;
                    db.SaveChanges();
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("List", "Exercise", new { Id = trainingId });
            }
        }

        [Authorize]
        public ActionResult Example(int id)
        {
            ViewBag.ident = id;
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Id.Equals(1)).FirstOrDefault();
                if (user != null)
                {
                    var exer = db.ExerciseAdmins.ToList();
                    return View(exer);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult ExampleAdd(int id, int idx)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var training = db.Trainings.Where(m => m.Id == idx).FirstOrDefault();
                var exAdm = db.ExerciseAdmins.Where(m => m.Id == id).FirstOrDefault();
                Exercise exercise = new Exercise();
                exercise.ExerciseName = exAdm.ExerciseName;
                exercise.VideoLink = exAdm.VideoLink;
                exercise.BurnedCalories = exAdm.BurnedCalories;
                exercise.Description = exAdm.Description;
                exercise.TrainingId = training.Id;
                training.BurnedCalories += exAdm.BurnedCalories;
                training.ExerciseCount += 1;
                training.Exercises.Add(exercise);
                db.SaveChanges();
            }
            return RedirectToAction("List", "Exercise", new { Id = idx });
        }
    }
}