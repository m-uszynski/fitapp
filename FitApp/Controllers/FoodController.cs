using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitApp.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
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
        public ActionResult Add(Food model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();

                if (user != null)
                {
                    if (user.UserType == "admin")
                    {
                        if (ModelState.IsValid)
                        {
                            model.MealId = 1;
                            db.Foods.Add(model);
                            db.SaveChanges();
                        }
                        else
                        {
                            return View(model);
                        }
                        return RedirectToAction("Composition", "Meal", new { Id = 1 });
                    }
                    else
                    {
                        return RedirectToAction("Permission", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var food = db.Foods.Where(m => m.Id == Id).FirstOrDefault();
                if (food != null)
                {
                    ViewBag.FoodName = food.FoodName;
                    ViewBag.Calories = food.Calories;
                    ViewBag.Fat = food.Fat;
                    ViewBag.Proteins = food.Proteins;
                    ViewBag.Carbs = food.Carbs;
                }
                else
                {
                    return RedirectToAction("Add", "Food");
                }


            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Food model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var food = db.Foods.Where(m => m.Id == model.Id).FirstOrDefault();
                if (food != null)
                {
                    food.FoodName = model.FoodName;
                    food.Calories = model.Calories;
                    food.Fat = model.Fat;
                    food.Proteins = model.Proteins;
                    food.Carbs = model.Carbs;

                    db.SaveChanges();

                    return RedirectToAction("Composition", "Meal", new { Id = 1 });
                }
                else
                {
                    return RedirectToAction("Add", "Food");
                }


            }


        }

        [Authorize]
        public ActionResult Products()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var meal = db.Meals.Where(u => u.Id.Equals(1)).FirstOrDefault();
                if (meal != null)
                {
                    var food = db.Foods.Where(u => u.MealId.Equals(meal.Id)).ToList();
                    return View(food);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
        }

        [Authorize]
        public ActionResult Select(int Id)
        {
            int mId = (int)TempData["MealId"];
            TempData.Keep("MealId");
            using (FitAppContext db = new FitAppContext())
            {
                var food = db.Foods.Where(f => f.Id.Equals(Id)).FirstOrDefault();
                food.MealId = mId;
                db.Foods.Add(food);

                var meal = db.Meals.Where(m => m.Id.Equals(mId)).FirstOrDefault();
                meal.MealCalories += food.Calories;
                meal.MealCarbs += food.Carbs;
                meal.MealFat += food.Fat;
                meal.MealProteins += food.Proteins;
               
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }
    }
}