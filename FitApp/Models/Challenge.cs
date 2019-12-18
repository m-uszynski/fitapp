using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(ChallengeMetadata))]
    public class Challenge
    {
        public Challenge()
        {
            this.Tasks = new HashSet<Task>();
        }


        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime EndDate { get; set; }
        public int GroupId { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual Group Groups { get; set; }
   


    }

    public class ChallengeMetadata
    {
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2020", ErrorMessage = "Data musi być pomiędzy {1} a {2}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Temat")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie tematu grupy jest wymagane")]
        public string Subject { get; set; }

        [Display(Name = "Identyfikator grupy")]
        public int GroupId { get; set; }
    }
}