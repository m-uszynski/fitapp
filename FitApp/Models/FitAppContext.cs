using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace FitApp.Models
{
    public class FitAppContext : DbContext
    {
        public FitAppContext() : base("FitApp")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Measurment> Measurments { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<ExerciseAdmin> ExerciseAdmins { get; set; }


        public System.Data.Entity.DbSet<FitApp.Models.Food> Foods { get; set; }

        public System.Data.Entity.DbSet<FitApp.Models.Meal> Meals { get; set; }

        public System.Data.Entity.DbSet<FitApp.Models.Group> Groups { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                    ); // Add the original exception as the innerException
            }
        }
    }
}