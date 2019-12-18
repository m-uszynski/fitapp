using FitApp.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class MealController : Controller
    {
        // GET: Meal
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult PdfView()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if(user!=null)
                {
                    var meals = db.Meals.Where(m => m.UserId == user.Id).ToList();
                    return View(meals);
                }
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult PdfPartialView(int id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var foods = db.Foods.Where(m => m.MealId == id).ToList();
                var report = new PartialViewAsPdf("~/Views/Meal/PdfPartialView.cshtml", foods);
                return report;
            }
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("PdfView");
            return report;
        }



        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(Meal model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
                
                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.UserId = user.Id;
                        db.Meals.Add(model);
                        db.SaveChanges();
                    }
                    else
                    {
                        return View(model);
                    }
                    return RedirectToAction("List", "Meal");
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
                    var meals = db.Meals.Where(u => u.UserId.Equals(user.Id)).ToList();
                    return View(meals);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult AdminMeals()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Id.Equals(1)).FirstOrDefault();
                if (user != null)
                {
                    var meals = db.Meals.Where(u => u.UserId.Equals(user.Id)).ToList();
                    meals.RemoveAt(0);
                    return View(meals);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult Meals(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                ViewBag.uType = user.UserType;
                if (user != null)
                {
                    var meals = db.Meals.Where(u => u.UserId.Equals(user.Id)).ToList();
                    return View(meals);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult Composition(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
                ViewBag.uType = user.UserType;

                var meal = db.Meals.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                if (meal != null)
                {
                    var food = db.Foods.Where(u => u.MealId.Equals(meal.Id)).ToList();
                    ViewBag.mName = meal.MealName;
                    return View(food);
                }
                else
                {
                    return RedirectToAction("Add", "Meal");
                }
            }
        }

        [Authorize]
        public ActionResult Users()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var users = db.Users.ToList();
                if (users != null)
                {
                    return View(users);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            return RedirectToAction("Add", "Meal");
        }

        [Authorize]
        public ActionResult AddProduct()
        {
            return RedirectToAction("Add", "Food");
        }

        [Authorize]
        public ActionResult UsersList()
        {
            return RedirectToAction("Users", "Meal");
        }

        [Authorize]
        public ActionResult UserMeals()
        {
            return RedirectToAction("Meals", "Meal");
        }

        [Authorize]
        public ActionResult AdminList()
        {
            return RedirectToAction("AdminMeals", "Meal");
        }

        [Authorize]
        public ActionResult EditProduct(Food model)
        {
            return RedirectToAction("Edit", "Food", model);
        }


        [Authorize]
        public ActionResult Compose()
        {
            return RedirectToAction("Products", "Food");
        }

        [Authorize]
        public ActionResult Details(int Id)
        {
            TempData["MealId"] = Id;
            return RedirectToAction("Composition", "Meal", new { Id });
        }

        [Authorize]
        public ActionResult DeleteMeal(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var meal = db.Meals.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                db.Meals.Remove(meal);
                db.SaveChanges();
                return RedirectToAction("List", "Meal");
            }
        }

        [Authorize]
        public ActionResult DeleteFood(int Id)
        {
            int mId = (int)TempData["MealId"];
            TempData.Keep("MealId");
            using (FitAppContext db = new FitAppContext())
            {
                int id_ = Id;
                var food = db.Foods.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                db.Foods.Remove(food);

                var meal = db.Meals.Where(m => m.Id.Equals(mId)).FirstOrDefault();
                meal.MealCalories -= food.Calories;
                meal.MealCarbs -= food.Carbs;
                meal.MealFat -= food.Fat;
                meal.MealProteins -= food.Proteins;


                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        [Authorize]
        public ActionResult SelectMeal(Meal meal)
        {
            return RedirectToAction("SetDate", "Meal", meal);
        }

        [Authorize]
        [HttpGet]
        public ActionResult SetDate(int Id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetDate(Meal model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
                var meal = db.Meals.Where(m => m.Id == model.Id).FirstOrDefault();
                if (meal != null)
                {
                    meal.MealDate = model.MealDate;
                    meal.MealTime = model.MealTime;
                    meal.UserId = user.Id;
                    db.Meals.Add(meal);

                    var foods = db.Foods.Where(m => m.MealId == model.Id).ToList();
                    foreach (var item in foods)
                    {
                        item.MealId = meal.Id;
                        db.Foods.Add(item);
                    }

                    db.SaveChanges();

                    return RedirectToAction("List", "Meal");
                }
                else
                {
                    return RedirectToAction("Add", "Meal");
                }
            }
        }

    }
}