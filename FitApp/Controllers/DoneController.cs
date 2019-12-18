using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FitApp.Controllers
{
    [Authorize]
    public class DoneController : ApiController
    {
        private FitAppContext db;

        public DoneController()
        {
            db = new FitAppContext();
        }

        [HttpPost]
        public IHttpActionResult Done([FromBody]int TaskId)
        {
            var task = db.Tasks.Where(m => m.Id == TaskId).FirstOrDefault();
            var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
            if (!user.TasksDone.Contains(task))
            {
                user.TasksDone.Add(task);
                if (!task.UsersWhoDid.Contains(user))
                {
                    task.UsersWhoDid.Add(user);
                }
                else
                {
                    return BadRequest("Zadanie jest zrealizowane");
                }
            }
            else
            {
                return BadRequest("Zadanie jest zrealizowane");
            }
            db.SaveChanges();

            return Ok();
        }
    }

    
}
