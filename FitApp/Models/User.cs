using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(UserMetadata))]
    public class User
    {

        public User()
        {
            this.Trainings = new HashSet<Training>();
            this.Groups = new HashSet<Group>();
            this.Measurments = new HashSet<Measurment>();
            this.Meals = new HashSet<Meal>();
            this.TasksDone = new HashSet<Task>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsEmailVerified { get; set; }
        public System.Guid ActivationCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public string Sex { get; set; }


        public virtual ICollection<Training> Trainings { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Measurment> Measurments { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
        public virtual ICollection<Task> TasksDone { get; set; }



    }

    public class UserMetadata
    {
        [Display(Name = "Imię")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie imienia jest wymagane")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie nazwiska jest wymagane")]
        public string Surname { get; set; }
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie email'a jest wymagane")]
        public string Email { get; set; }
        [Display(Name="Login")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Podanie loginu jest wymagane")]
        public string Login { get; set; }
        [Display(Name = "Data urodzenia")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2019", ErrorMessage = "Data musi być pomiędzy {1} a {2}")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDay { get; set; }
        [Display(Name = "Hasło")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Hasło musi mieć minimum 6 wyrazów")]
        public string Password { get; set; }
        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Podane hasła nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Płeć")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie płci jest wymagane")]
        public string Sex { get; set; }
    }
}