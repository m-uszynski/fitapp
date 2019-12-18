using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(ExerciseMetadata))]
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string VideoLink { get; set; }
        public float BurnedCalories { get; set; }
        public string Description { get; set; }
        public int TrainingId { get; set; }

        public virtual Training Trainings { get; set; }
    }

    public class ExerciseMetadata
    {
        [Display(Name = "Nazwa ćwiczenia")]
        public string ExerciseName { get; set; }
        [Display(Name = "Link video")]
        public string VideoLink { get; set; }
        [Display(Name = "Spalane kalorie")]
        public float BurnedCalories { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}