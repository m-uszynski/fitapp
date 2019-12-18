using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FitApp.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }



        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Group model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (FitAppContext db = new FitAppContext())
                {
                    var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                    if (user != null)
                    {
                        Group group = new Group();
                        group.AdminId = user.Id;
                        group.GroupName = model.GroupName;
                        group.GroupType = model.GroupType;
                        group.MembersCount = 1;

                        user.Groups.Add(group);
                        db.SaveChanges();

                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }
                }
                return RedirectToAction("Mine", "Group");
            }
            else
            {
                message = "Uzupełnij dane";

            }
            ViewBag.Message = message;
            return View(model);
            
        }

        public ActionResult Mine()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if(user!=null)
                {
                    ViewBag.UserId = user.Id;
                    var groups = user.Groups.ToList();
                    return View(groups);
                    

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
               
        }

        public ActionResult Delete(int Id)
        {

            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == Id).FirstOrDefault();
                var challenges = group.Challenges.ToList();

                foreach(var ch in challenges)
                {
                    var tasks = ch.Tasks.ToList();
                    foreach(var t in tasks)
                    {
                        foreach(var u in t.UsersWhoDid)
                        {
                            TaskRepository.TaskEdit(t, u.Id, ch.Subject);
                        }
                    }
                }

                db.Groups.Remove(group);
                db.SaveChanges();

                return RedirectToAction("Mine", "Group");
            }
            /*
              using (FitAppContext db = new FitAppContext())
            {
                var challenge = db.Challenges.Where(m => m.Id == Id).FirstOrDefault();
                var groupId = challenge.GroupId;

                var tasks = challenge.Tasks.ToList();

                foreach (var t in tasks)
                {
                    foreach (var u in t.UsersWhoDid)
                    {
                        TaskRepository.TaskEdit(t, u.Id, challenge.Subject);
                    }

                }

                db.Challenges.Remove(challenge);
                db.SaveChanges();

                return RedirectToAction("List", "Challenge", new { Id = groupId });
            }
            */


        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == Id).FirstOrDefault();
                if(group!=null)
                {
                    ViewBag.GroupName = group.GroupName;
                    ViewBag.GroupType = group.GroupType;
                }
                else
                {
                    return RedirectToAction("Create", "Group");
                }

                
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Group model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == model.Id).FirstOrDefault();
                if(group!=null)
                {
                    group.GroupName = model.GroupName;
                    group.GroupType = model.GroupType;

                    db.SaveChanges();

                    return RedirectToAction("Mine", "Group");
                }
                else
                {
                    return RedirectToAction("Create", "Group");
                }

                
            }


        }


        public ActionResult List()
        {
            var message = "";
            using (FitAppContext db = new FitAppContext())
            {
                var groups = db.Groups.ToList();
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if(user!=null)
                {
                    ViewBag.UserId = user.Id;
                    List<Group> groupEnroled = new List<Group>();
                    foreach (var g in groups)
                    {
                        groupEnroled.Add(user.Groups.Where(m => m.Id == g.Id).FirstOrDefault());
                    }
                   
                    List<Group> groupList = new List<Group>();

                    foreach (var g in groups)
                    {
                        if (!groupEnroled.Contains(g))
                        {
                            groupList.Add(g);
                        }
                    }

                    if (groupList != null)
                    {
                        return View(groupList);
                    }
                    else
                    {
                        message = "Nie ma żadnych grup. Stwórz swoją w zakładce Stwórz grupę";
                    }
                }
                else
                {                  
                    return View(groups);
                }

            }
            return View();
        }


        [Authorize]
        public ActionResult Enrol(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == Id).FirstOrDefault();
                if(group!=null)
                {
                    var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                    group.Users.Add(user);
                    group.MembersCount++;
                    user.Groups.Add(group);
                    db.SaveChanges();
                    ViewBag.Message="Udalo sie dodac";
                    return RedirectToAction("Mine","Group");
                }
                else
                {
                    ViewBag.Message = "Coś poszło nie tak";
                    return RedirectToAction("List", "Group");
                }

            }
            
        }
        [Authorize]
        public ActionResult Unenrol(int Id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == Id).FirstOrDefault();
                if(group!=null)
                {
                    var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                    group.Users.Remove(user);
                    group.MembersCount--;
                    db.SaveChanges();
                    return RedirectToAction("Mine", "Group");
                }
                else
                {
                    return RedirectToAction("Mine", "Group");
                }
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var group = db.Groups.Where(m => m.Id == id).FirstOrDefault();
                ViewBag.AdminId = group.AdminId;
                return View(group.Users);
            }

        }


        [Authorize]
        public ActionResult Block(int UserId)
        {
            using (FitAppContext db = new FitAppContext())
            {
                try
                {
                    var user = db.Users.Where(m => m.Id == UserId).FirstOrDefault();
                    var groupId = int.Parse(Request.UrlReferrer.Segments.Last());

                    var group = db.Groups.Where(m => m.Id == groupId).FirstOrDefault();
                    group.Users.Remove(user);
                    group.MembersCount--;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {

                }

                return RedirectToAction("Mine","Group");

            }
        }

        [HttpGet]
        public JsonResult PublicGroups()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var item = (from items in db.Groups where items.GroupType.StartsWith("Public") select new {Id=items.Id, Nazwa = items.GroupName, ilosc = items.MembersCount }).ToList();
                                       
                return Json(item,JsonRequestBehavior.AllowGet); 
            }
        }

    

    }
}