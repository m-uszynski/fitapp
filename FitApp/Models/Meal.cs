using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FitApp.Models
{
    [MetadataType(typeof(MealMetadata))]
    public class Meal
    {
        public Meal()
        {
            this.Foods = new HashSet<Food>();
        }
        public int Id { get; set; }
        public string MealName { get; set; }
        public float MealCalories { get; set; }
        public float MealProteins { get; set; }
        public float MealFat { get; set; }
        public float MealCarbs { get; set; }
        public DateTime MealDate { get; set; }
        public DateTime MealTime { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
        public virtual User Users { get; set; }

    }

    public class MealMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data")]
        public DateTime MealDate { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Godzina")]
        public DateTime MealTime { get; set; }
        [Display(Name = "Nazwa")]
        public string MealName { get; set; }
        [Display(Name = "Kalorie")]
        public string MealCalories { get; set; }
        [Display(Name = "Białko")]
        public string MealProteins { get; set; }
        [Display(Name = "Tłuszcze")]
        public string MealFat { get; set; }
        [Display(Name = "Węglowodany")]
        public string MealCarbs { get; set; }
    }
}