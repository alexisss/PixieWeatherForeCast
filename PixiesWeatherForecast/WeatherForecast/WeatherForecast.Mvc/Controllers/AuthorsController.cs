using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Models;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;

namespace WeatherForecast.Mvc.Controllers
{
    public class AuthorsController : BaseController
    {
        public ActionResult AllAuthors()
        {
            string authorRoleString = "Author";
            var allAuthors = Data.Users.All()
                .Where(u => u.Roles.Any(r => r.Role.Name == authorRoleString))
                .Select(AuthorViewModel.FromUser).ToList();

            return View(allAuthors);
        }

        public JsonResult GetAuthors([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Users.All()
                .Select(ForecastViewModel.FromForecast);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuthorDetails(string username)
        {
            var author = Data.Users.All().FirstOrDefault(x => x.UserName == username);

            if (author == null)
            {
                throw new Exception("No such author.");
            }

            ExtendedAuthorViewModel authorViewModel = new ExtendedAuthorViewModel()
            {
                AuthorId = author.Id,
                Descripiton = author.Description,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ProfilePictureUrl = author.PictureUrl,
                Rating = 10,
                UserName = author.UserName,
            };

            bool canEdit = User.IsInRole("Admin") || User.Identity.GetUserName() == username;

            ViewBag.CanEdit = canEdit;


            return View(authorViewModel);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult TogleUser(string username)
        {
            var userDb = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (userDb == null)
            {
                string message = string.Format("No such user(0).", username);
                throw new ArgumentException(message);
            }

            var authorRole = userDb.Roles.FirstOrDefault(r => r.Role.Name == "Author");
            bool isAuthor = authorRole != null;
            if (isAuthor)
            {
                userDb.Roles.Remove(authorRole);
                isAuthor = false;
            }
            else
            {
                var dbAuthorRole = this.Data.Roles.All().FirstOrDefault(r => r.Name == "Author");
                UserRole userRole = new UserRole
                {
                    Role = dbAuthorRole,
                    User = userDb
                };
                userDb.Roles.Add(userRole);
                isAuthor = true;
            }

            this.Data.SaveChanges();

            return Content(isAuthor.ToString());
        }
    }
}