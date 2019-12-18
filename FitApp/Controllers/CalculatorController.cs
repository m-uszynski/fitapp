using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class CalculatorController : Controller
    {
        // GET: Calculator
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Bmi()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                try
                {
                    var measurment = user.Measurments.Last();

                    if (measurment != null)
                    {
                        ViewBag.Height = measurment.Height.ToString();
                        ViewBag.Weight = measurment.Weight.ToString();
                    }
                    else
                    {
                        ViewBag.Message = "Uzupełnij pomiar";
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Uzupełnij pomiar";
                }



            }
                return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Bmi(Measurment model)
        {
            float weight = model.Weight;
            float height = model.Height/100;

            float pattern = weight / (height * height);

            string message = "Twoje BMI wynosi " + pattern;
            ViewBag.Height = model.Height.ToString();
            ViewBag.Weight = model.Weight.ToString();
            ViewBag.Message = message;

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Bmr()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();                
                int ageInYears = (Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) -
                  Int32.Parse(user.BirthDay.ToString("yyyyMMdd"))) / 10000;

                if (user!=null)
                {
                    ViewBag.Sex = user.Sex;
                    ViewBag.Age = ageInYears;
                    try
                    {
                        var measurment = user.Measurments.Last();
                        ViewBag.Weight = measurment.Weight.ToString();
                        ViewBag.Height = measurment.Height.ToString();
                        ViewBag.ActivityLevel = measurment.ActivityLevel;


                    }
                    catch(Exception ex)
                    {
                        ViewBag.Message = "Uzupełnij pomiar";
                    }
                    

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }

                return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Bmr(Measurment model)
        {
            var message = "";
            using (FitAppContext db = new FitAppContext())
            {

                try
                {
                    var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                    if (user != null)
                    {
                        var weight = model.Weight;
                        var height = model.Height;
                        int ageInYears = (Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) - Int32.Parse(user.BirthDay.ToString("yyyyMMdd"))) / 10000;
                        ViewBag.Weight = model.Weight.ToString();
                        ViewBag.Height = model.Height.ToString();
                        ViewBag.ActivityLevel = model.ActivityLevel;
                        ViewBag.Age = ageInYears;
                        ViewBag.Sex = user.Sex;

                        float bmr = 0;

                        //metoda Hattisa-Benedicta
                        if (user.Sex == "Female")
                        {
                            bmr = (float)((9.6 * weight) + (1.8 * height) - (4.7 * ageInYears) + 655);
                        }
                        else if (user.Sex == "Male")
                        {
                            bmr = (float)((13.7 * weight) + (5 * height) - (6.76 * ageInYears) + 66);
                        }
                        float multipler = 0;


                        switch (model.ActivityLevel)
                        {
                            case "Znikomy":
                                multipler = (float)1.2;
                                break;
                            case "Niski":
                                multipler = (float)1.4;
                                break;
                            case "Średni":
                                multipler = (float)1.6;
                                break;
                            case "Wysoki":
                                multipler = (float)1.8;
                                break;
                            case "Bardzo Wysoki":
                                multipler = (float)2.0;
                                break;
                            default:
                                message = "Zły poziom aktywności";
                                break;
                        }
                        float activityBmr = (float)(bmr * multipler);

                        ViewBag.BMR = bmr;
                        ViewBag.ActivityBMR = activityBmr;



                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
                catch (Exception ex)
                {
                    message = "Uzupełnij dane";
                   
                }
                
               
            }
            ViewBag.Message = message;

                return View(model);
        }
    }
}