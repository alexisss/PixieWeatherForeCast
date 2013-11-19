using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using Kendo.Mvc.Extensions;
using WeatherForecast.Mvc.Controllers;
using WeatherForecast.Mvc.Areas.Administration.ViewModels;
using WeatherForecast.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WeatherForecast.Mvc.Models;
using System.IO;

namespace WeatherForecast.Mvc.Areas.Administration.Controllers
{
    public class ExtendedUsersController : AdminsController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadUsers([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Users.All().Select(AdminExtendedUser.FromExtendedUser);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUsers([DataSourceRequest] DataSourceRequest request, AdminExtendedUser user)
        {
            if (user != null && ModelState.IsValid)
            {
                var newUser = new ExtendedUser
                {                   
                   UserName = user.UserName,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Description = user.Description,
                   PictureUrl = user.PictureUrl.ToString()     
                   
                };
                
                this.Data.Users.Add(newUser);
                this.Data.SaveChanges();

                user.Id = newUser.Id;
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUsers([DataSourceRequest] DataSourceRequest request, AdminExtendedUser user)
        {
            var existingUser = this.Data.Users.All().Where(u => u.Id == user.Id).FirstOrDefault();

            if (user != null && ModelState.IsValid)
            {
                existingUser.UserName = user.UserName;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Description = user.Description;
                ViewBag.UserName = user.UserName;                

                this.Data.SaveChanges();
            }

            return Json((new[] { user }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteUsers([DataSourceRequest] DataSourceRequest request, AdminExtendedUser user)
        {
            var existingUser = this.Data.Users.All().Where(u => u.Id == user.Id).FirstOrDefault();

            this.Data.Users.Delete(existingUser);
            this.Data.SaveChanges();

            return Json(new[] { user }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadedFiles(IEnumerable<HttpPostedFileBase> upload)
        {
            if (upload != null)
            {
                foreach (var file in upload)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/img/Account"), fileName);

                    string userName = ViewBag.UserName;
                    ExtendedUser user = this.Data.Users.All().Where(u => u.UserName == userName).FirstOrDefault();
                    user.PictureUrl = "../img/Account/" + fileName;
                    ViewBag.picture = "../img/Account/" + fileName;
                    Data.SaveChanges();
                    file.SaveAs(physicalPath);                    
                }
            }

            return Content("");
        }
	}
}