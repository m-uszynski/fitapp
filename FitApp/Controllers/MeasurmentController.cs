using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class MeasurmentController : Controller
    {
        // GET: Measurment
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Add(Measurment model)
        {
            if(ModelState.IsValid)
            {
                using (FitAppContext db = new FitAppContext())
                {
                    var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                    if (user != null)
                    {
                        model.UserId = user.Id;
                        user.Measurments.Add(model);
                        db.SaveChanges();

                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }
                    return RedirectToAction("History", "Measurment");
                }
            }

            return View(model);
            

        }

        public ActionResult History()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if(user!=null)
                {
                    var measurment = db.Measurments.Where(m=>m.UserId==user.Id).ToList();
                    return View(measurment);


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
                var measurment = db.Measurments.Where(m => m.Id == Id).FirstOrDefault();
                db.Measurments.Remove(measurment);
                db.SaveChanges();
                return RedirectToAction("History", "Measurment");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var measurment = db.Measurments.Where(m => m.Id == Id).FirstOrDefault();
                if(measurment!=null)
                {
                    ViewBag.MeasurmentDate = measurment.MeasurmentDate.ToShortDateString();
                    ViewBag.Weight = measurment.Weight;
                    ViewBag.Height = measurment.Height;
                    ViewBag.BicepsCircuit = measurment.BicepsCircuit;
                    ViewBag.ThighCircuit = measurment.ThighCircuit;
                    ViewBag.WeistCircuit = measurment.WeistCircuit;
                    ViewBag.ChestCircuit = measurment.ChestCircuit;
                    ViewBag.HipsCircuit = measurment.HipsCircuit;
                    ViewBag.ShouldersCircuit = measurment.ShouldersCircuit;
                    ViewBag.ActivityLevel = measurment.ActivityLevel;
                }
                else
                {
                    return RedirectToAction("Add", "Measurment");
                }
            }
                return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Measurment model)
        {
            var message = "";
            using (FitAppContext db = new FitAppContext())
            {
                
                var measurment = db.Measurments.Where(m => m.Id == model.Id).FirstOrDefault();
                if(measurment!=null)
                {
                    measurment.MeasurmentDate = model.MeasurmentDate;
                    measurment.Weight = model.Weight;
                    measurment.Height = model.Height;
                    measurment.BicepsCircuit = model.BicepsCircuit;
                    measurment.ThighCircuit = model.ThighCircuit;
                    measurment.WeistCircuit = model.WeistCircuit;
                    measurment.ChestCircuit = model.ChestCircuit;
                    measurment.HipsCircuit = model.HipsCircuit;
                    measurment.ShouldersCircuit = model.ShouldersCircuit;
                    measurment.ActivityLevel = model.ActivityLevel;
                    db.SaveChanges(); 
                    message = "Dane zostały zaktualizowane";
                }
                else
                {
                    message = "Nie udało się zaktualizować danych";
                    return View(model);
                }
            }
            ViewBag.Message = message;
            return RedirectToAction("History","Measurment");
        }



    }
}