using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace FitApp.Controllers
{
    public class UserController : Controller
    {
        #region Registration
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User user)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                #region Email/Login existance
                var doExistEmail = IsEmailExists(user.Email);
                var doExistLogin = DoLoginExists(user.Login);
                if (doExistLogin)
                {
                    ModelState.AddModelError("LoginExists", "Podany login już istnieje");
                    return View(user);
                }
                if (doExistEmail)
                {
                    ModelState.AddModelError("EmailExists", "Podany email już istnieje");
                    return View(user);
                }

                #endregion
                #region Generate Activation COde
                user.ActivationCode = Guid.NewGuid();
                #endregion
                #region Password hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save to Dataase
                using (FitAppContext db = new FitAppContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    message = "Rejestracja udana. Aktywuj swoje konto";
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";

            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);
        }
        #endregion
        #region AccountVerification
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (FitAppContext db = new FitAppContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var v = db.Users.Where(m => m.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }

            }
            ViewBag.Status = true;
            return View();
        }
        #endregion
        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(u => u.Login == model.Login).FirstOrDefault();

                if (user != null)
                {
                    if (string.Compare(Crypto.Hash(model.Password), user.Password) == 0)
                    {
                        int timeout = 20;
                        var ticket = new FormsAuthenticationTicket(user.Login, false, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(model);
                    }
                }
            }
            return View();
        }

        #endregion
        #region Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region PasswordReset
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            bool status = false;
            string message = "";
            using (FitAppContext db = new FitAppContext())
            {
                var account = db.Users.Where(m => m.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Check your email for verification link";
                }
                else
                {
                    message = User.Identity.Name;
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var account = db.Users.Where(m => m.ResetPasswordCode == id).FirstOrDefault();
                if (account != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (FitAppContext db = new FitAppContext())
                {
                    var account = db.Users.Where(m => m.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (account != null)
                    {
                        account.Password = Crypto.Hash(model.NewPassword);
                        account.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Password updated successfully";

                    }

                }

            }
            else
            {
                message = "Something went wrong";
            }
            ViewBag.Message = message;
            return View(model);
        }
        #endregion
        #region DataUpdate
        [HttpGet]
        public ActionResult Data()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.UserSurname = user.Surname;
                    ViewBag.UserBirthDay = user.BirthDay.ToShortDateString();
                    ViewBag.UserEmail = user.Email;
                    ViewBag.UserSex = user.Sex;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Data(User model)
        {
            var message = "";
            
            using (FitAppContext db = new FitAppContext())
            {           
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.UserName = model.Name;
                    ViewBag.UserSurname = model.Surname;
                    ViewBag.UserBirthDay = model.BirthDay.ToShortDateString();
                    ViewBag.UserEmail = model.Email;
                    ViewBag.UserSex = model.Sex;
                    if (user.IsEmailVerified == true)
                    {
                        user.Name = model.Name;
                        user.Surname = model.Surname;
                        user.BirthDay = model.BirthDay;
                        user.Sex = model.Sex;
                        if(model.Email!=user.Email)
                        {
                            user.Email = model.Email;
                            user.IsEmailVerified = false;
                            var code = Guid.NewGuid();
                            user.ActivationCode = code;
                            SendVerificationLinkEmail(model.Email, code.ToString());
                        }


                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Dane zostały zaktualizowane";

                    }
                    else
                    {
                        ViewBag.UserName = user.Name;
                        ViewBag.UserSurname = user.Surname;
                        ViewBag.UserBirthDay = user.BirthDay.ToShortDateString();
                        ViewBag.UserEmail = user.Email;
                        ViewBag.UserSex = user.Sex;
                        message = "Potwierdź email w celu zmiany danych";
                        ViewBag.Message = message;
                    }

                }
                else
                {
                    message = "Coś poszło nie tak";
                }


            }

            ViewBag.Message = message;
            return View();
        }
        #endregion

        [Authorize]
        public ActionResult History()
        {
            using (FitAppContext db = new FitAppContext())
            {
                var user = db.Users.Where(m => m.Login == User.Identity.Name).FirstOrDefault();
                var taskHistory = db.TaskHistories.Where(m => m.UserId == user.Id).ToList();
                return View(taskHistory);
            }
                
        }


        [NonAction]
        public bool IsEmailExists(string email)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var v = db.Users.Where(a => a.Email == email).FirstOrDefault();
                return v == null ? false : true;
            }
        }
        [NonAction]
        public bool DoLoginExists(string login)
        {
            using (FitAppContext db = new FitAppContext())
            {
                var v = db.Users.Where(m => m.Login == login).FirstOrDefault();
                return v == null ? false : true;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationcode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationcode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("fitappusers2@gmail.com", "Wiadomość od FitApp");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Qwerty!2345";
            string subject = "";
            string body = "";


            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is created";
                body = "Siema. Click to verify" + "<br/><br/><a href=" + link + ">" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Clik on link to restore password<br/><a href=" + link + ">ResetPasswordLink</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }









    }
}