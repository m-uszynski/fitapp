using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace FitApp.Models
{
    [MetadataType(typeof(TaskMetadata))]
    public class Task
    {
        public Task()
        {
            this.UsersWhoDid = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime RealizationTime { get; set; }
        public int ChallengeId { get; set; } 

        public virtual Challenge Challanges { get; set; }
        public virtual ICollection<User> UsersWhoDid { get; set; }
    }

    [MetadataType(typeof(TaskHistoryMetadata))]
    public class TaskHistory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime RealizationTime { get; set; }
        public int ChallengeId { get; set; }
        public string ChallengeName { get; set; }
        public int UserId { get; set; }

    }
 
    public class TaskRepository
    {
        public static void TaskEdit(Task taskToEdit,int userId,string challengeName)
        {
            using (FitAppContext db = new FitAppContext())
            {
                db.TaskHistories.Add(new TaskHistory()
                {
                    ChallengeId = taskToEdit.ChallengeId,
                    Description = taskToEdit.Description,
                    ChallengeName = challengeName,
                    Id = taskToEdit.Id,
                    UserId = userId,
                    RealizationTime = taskToEdit.RealizationTime

                });
                db.SaveChanges();
            }



        }
    }

    public class TaskMetadata
    {
        [Display(Name = "Data realizacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2020", ErrorMessage = "Data musi być pomiędzy {1} a {2}")]
        public DateTime RealizationTime { get; set; }

        [Display(Name = "Opis zadania")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie opisu zadania jest wymagane")]
        public string Description { get; set; }

        [Display(Name = "Identyfikator wyzwania")]
        public int ChallengeId { get; set; }

    }

    public class TaskHistoryMetadata
    {
        [Display(Name = "Opis zadania")]
        public string Description { get; set; }

        [Display(Name = "Data realizacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2020", ErrorMessage = "Data musi być pomiędzy {1} a {2}")]
        public DateTime RealizationTime { get; set; }

        [Display(Name = "Nazwa wyzwania")]
        public string ChallengeName { get; set; }

        [Display(Name = "Identyfikator wyzwania")]
        public int ChallengeId { get; set; }
    }
}