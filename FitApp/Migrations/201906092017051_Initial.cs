namespace FitApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Challenges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MembersCount = c.Int(nullable: false),
                        GroupName = c.String(nullable: false),
                        AdminId = c.Int(nullable: false),
                        GroupType = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserType = c.String(),
                        Email = c.String(nullable: false),
                        BirthDay = c.DateTime(nullable: false),
                        ConfirmPassword = c.String(),
                        IsEmailVerified = c.Boolean(nullable: false),
                        ActivationCode = c.Guid(nullable: false),
                        ResetPasswordCode = c.String(),
                        Sex = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Meals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MealName = c.String(),
                        MealCalories = c.Single(nullable: false),
                        MealProteins = c.Single(nullable: false),
                        MealFat = c.Single(nullable: false),
                        MealCarbs = c.Single(nullable: false),
                        MealDate = c.DateTime(nullable: false),
                        MealTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FoodName = c.String(),
                        Calories = c.Single(nullable: false),
                        Fat = c.Single(nullable: false),
                        Proteins = c.Single(nullable: false),
                        Carbs = c.Single(nullable: false),
                        MealId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meals", t => t.MealId, cascadeDelete: true)
                .Index(t => t.MealId);
            
            CreateTable(
                "dbo.Measurments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeasurmentDate = c.DateTime(nullable: false),
                        Weight = c.Single(nullable: false),
                        Height = c.Single(nullable: false),
                        ActivityLevel = c.String(),
                        BicepsCircuit = c.Single(nullable: false),
                        ThighCircuit = c.Single(nullable: false),
                        WeistCircuit = c.Single(nullable: false),
                        ChestCircuit = c.Single(nullable: false),
                        HipsCircuit = c.Single(nullable: false),
                        ShouldersCircuit = c.Single(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        RealizationTime = c.DateTime(nullable: false),
                        ChallengeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.ChallengeId, cascadeDelete: true)
                .Index(t => t.ChallengeId);
            
            CreateTable(
                "dbo.Trainings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BurnedCalories = c.Single(nullable: false),
                        TrainingTime = c.DateTime(nullable: false),
                        TrainingName = c.String(),
                        ExerciseCount = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                        VideoLink = c.String(),
                        BurnedCalories = c.Single(nullable: false),
                        Description = c.String(),
                        TrainingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainings", t => t.TrainingId, cascadeDelete: true)
                .Index(t => t.TrainingId);
            
            CreateTable(
                "dbo.ExerciseAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                        VideoLink = c.String(),
                        BurnedCalories = c.Single(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        RealizationTime = c.DateTime(nullable: false),
                        ChallengeId = c.Int(nullable: false),
                        ChallengeName = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.TaskUsers",
                c => new
                    {
                        Task_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Task_Id, t.User_Id })
                .ForeignKey("dbo.Tasks", t => t.Task_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Task_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Exercises", "TrainingId", "dbo.Trainings");
            DropForeignKey("dbo.TaskUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TaskUsers", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "ChallengeId", "dbo.Challenges");
            DropForeignKey("dbo.Measurments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Meals", "UserId", "dbo.Users");
            DropForeignKey("dbo.Foods", "MealId", "dbo.Meals");
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Challenges", "GroupId", "dbo.Groups");
            DropIndex("dbo.TaskUsers", new[] { "User_Id" });
            DropIndex("dbo.TaskUsers", new[] { "Task_Id" });
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.Exercises", new[] { "TrainingId" });
            DropIndex("dbo.Trainings", new[] { "UserId" });
            DropIndex("dbo.Tasks", new[] { "ChallengeId" });
            DropIndex("dbo.Measurments", new[] { "UserId" });
            DropIndex("dbo.Foods", new[] { "MealId" });
            DropIndex("dbo.Meals", new[] { "UserId" });
            DropIndex("dbo.Challenges", new[] { "GroupId" });
            DropTable("dbo.TaskUsers");
            DropTable("dbo.UserGroups");
            DropTable("dbo.TaskHistories");
            DropTable("dbo.ExerciseAdmins");
            DropTable("dbo.Exercises");
            DropTable("dbo.Trainings");
            DropTable("dbo.Tasks");
            DropTable("dbo.Measurments");
            DropTable("dbo.Foods");
            DropTable("dbo.Meals");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.Challenges");
        }
    }
}
