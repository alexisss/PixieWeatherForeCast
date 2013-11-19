using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Areas.Administration.ViewModels;
using WeatherForecast.Mvc.Controllers;
using Kendo.Mvc.Extensions;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Areas.Administration.Controllers
{
    public class AreasController : AdminsController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadArea([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Areas.All().Select(AdminAreas.FromArea);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateArea([DataSourceRequest] DataSourceRequest request, AdminAreas area)
        {
            if (area != null && ModelState.IsValid)
            {
                var newArea = new Area
                {
                    Name = area.Name    
                };

                this.Data.Areas.Add(newArea);                
                this.Data.SaveChanges();

                area.Id = newArea.Id;
            }

            return Json(new[] { area }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditArea([DataSourceRequest] DataSourceRequest request, AdminAreas area)
        {
            var existingArea = this.Data.Areas.GetById(area.Id);

            if (area != null && ModelState.IsValid)
            {
                existingArea.Name = area.Name;

                this.Data.SaveChanges();
            }

            return Json((new[] { area }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteArea([DataSourceRequest] DataSourceRequest request, AdminAreas area)
        {
            var existingArea = this.Data.Areas.GetById(area.Id);

            this.Data.Areas.Delete(existingArea);
            this.Data.SaveChanges();

            return Json(new[] { area }, JsonRequestBehavior.AllowGet);
        }
	}
}