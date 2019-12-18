namespace FitApp.Migrations
{
    using FitApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FitApp.Models.FitAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FitApp.Models.FitAppContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Users.AddOrUpdate(new User()
            {
                Id = 1,
                Name = "admin",
                Surname = "admin",
                Login = "admin",
                Password = Crypto.Hash("qwerty"),
                ConfirmPassword = Crypto.Hash("qwerty"),
                UserType = "admin",
                Email = "admin@amdin1233.com",
                BirthDay = new DateTime(1999, 1, 1),
                IsEmailVerified = false,
                ActivationCode = Guid.NewGuid(),
                Sex = "male"
            });

            context.Meals.AddOrUpdate(new Meal()
            {
                Id = 1,
                MealName = "",
                MealCalories = 0,
                MealProteins = 0,
                MealFat = 0,
                MealDate = new DateTime(2019, 1, 1),
                MealTime = Convert.ToDateTime("11:11"),
                MealCarbs = 0,
                UserId = 1
            });

            context.Foods.AddOrUpdate(x => x.Id,
                new Food() { Id = 1, MealId = 1, FoodName = "Pierœ z kurczaka",
                    Calories = 165, Proteins = 31, Fat = 4, Carbs = 0},
                new Food() { Id = 2, MealId = 1, FoodName = "Ry¿ bia³y",
                    Calories = 68, Proteins = 1, Fat = 0, Carbs = 15},
                new Food() { Id = 3, MealId = 1, FoodName = "Ry¿ br¹zowy",
                    Calories = 82, Proteins = 2, Fat = 1, Carbs = 17},
                new Food() { Id = 4, MealId = 1, FoodName = "Jajko kurze",
                    Calories = 70, Proteins = 6, Fat = 5, Carbs = 0},
                new Food() { Id = 5, MealId = 1, FoodName = "Oponka",
                    Calories = 194, Proteins = 6, Fat = 11, Carbs = 17
                });
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
