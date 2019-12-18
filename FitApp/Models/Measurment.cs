using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(MeasurmentMetadata))]
    public class Measurment
    {
        public int Id { get; set; }
        public DateTime MeasurmentDate { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string ActivityLevel { get; set; }
        public float BicepsCircuit { get; set; }
        public float ThighCircuit { get; set; }
        public float WeistCircuit { get; set; }
        public float ChestCircuit { get; set; }
        public float HipsCircuit { get; set; }
        public float ShouldersCircuit { get; set; }
        public int UserId { get; set; }

        public virtual User Users { get; set; } 

    }
    public class MeasurmentMetadata
    {
        [Display(Name ="Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2020", ErrorMessage = "Data musi być pomiędzy {1} a {2}")]
        public DateTime MeasurmentDate { get; set; }
        [Display(Name = "Waga (kg)")]
        [Required(AllowEmptyStrings =true)]
        public float Weight { get; set; }
        [Display(Name = "Wzrost (cm)")]
        public float Height { get; set; }
        [Display(Name = "Biceps (cm)")]
        public float BicepsCircuit { get; set; }
        [Display(Name = "Udo (cm)")]
        public float ThighCircuit { get; set; }
        [Display(Name = "Talia (cm)")]
        public float WeistCircuit { get; set; }
        [Display(Name = "Klatka piersiowa (cm)")]
        public float ChestCircuit { get; set; }
        [Display(Name = "Biodro (cm)")]
        public float HipsCircuit { get; set; }
        [Display(Name = "Barki (cm)")]
        public float ShouldersCircuit { get; set; }
        [Display(Name = "Poziom aktywsności")]
        public string ActivityLevel { get; set; }
    }
}