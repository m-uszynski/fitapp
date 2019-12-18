using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(ExerciseAdminMetadata))]
    public class ExerciseAdmin
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string VideoLink { get; set; }
        public float BurnedCalories { get; set; }
        public string Description { get; set; }
    }
    public class ExerciseAdminMetadata
    {
        [Display(Name = "Nazwa ćwiczenia")]
        public string ExerciseName { get; set; }
        [Display(Name = "Link Video")]
        public string VideoLink { get; set; }
        [Display(Name = "Spalane kalorie")]
        public float BurnedCalories { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}