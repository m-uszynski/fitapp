using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(FoodMetadata))]
    public class Food
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public float Calories { get; set; }
        public float Fat { get; set; }
        public float Proteins { get; set; }
        public float Carbs { get; set; }
        public int MealId { get; set; }

        public virtual Meal Meals { get; set; }
    }

    public class FoodMetadata
    {
        [Display(Name = "Nazwa")]
        public string FoodName { get; set; }
        [Display(Name = "Kalorie")]
        public string Calories { get; set; }
        [Display(Name = "Białko")]
        public string Proteins { get; set; }
        [Display(Name = "Tłuszcze")]
        public string Fat { get; set; }
        [Display(Name = "Węglowodany")]
        public string Carbs { get; set; }
    }
}