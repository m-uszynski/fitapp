using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(TrainingMetadata))]
    public class Training
    {
        public Training()
        {
            this.Exercises = new HashSet<Exercise>();
        }
        public int Id { get; set; }
        public float BurnedCalories { get; set; }
        public DateTime TrainingTime { get; set; }
        public string TrainingName { get; set; }
        public int ExerciseCount { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual User Users { get; set; }
    }

    public class TrainingMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Długość treningu")]
        public DateTime TrainingTime { get; set; }
        [Display(Name = "Spalane kalorie")]
        public float BurnedCalories { get; set; }
        [Display(Name = "Nazwa treningu")]
        public string TrainingName { get; set; }
        [Display(Name = "Ilość ćwiczeń")]
        public int ExerciseCount { get; set; }
    }
}