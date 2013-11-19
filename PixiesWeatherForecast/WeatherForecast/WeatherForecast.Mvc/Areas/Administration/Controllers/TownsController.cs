using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Models;
using Kendo.Mvc.Extensions;
using WeatherForecast.Mvc.Controllers;
using WeatherForecast.Mvc.Areas.Administration.ViewModels;
using WeatherForecast.Models;


namespace WeatherForecast.Mvc.Areas.Administration.Controllers
{
    public class TownsController : AdminsController
    {

        // GET: /Administration/Towns/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadTown([DataSourceRequest]DataSourceRequest request)
        {
            var result = this.Data.Towns.All().Select(AdminTowns.FromTown);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateTown([DataSourceRequest] DataSourceRequest request, AdminTowns town)
        {
            if (town != null && ModelState.IsValid)
            {
                var category = this.Data.Areas.GetById(town.AreaId);

                var newTown = new Town
                {
                    Name = town.Name,
                    AreaId = town.AreaId,
                    
                };

                this.Data.Towns.Add(newTown);                
                this.Data.SaveChanges();

                town.Id = newTown.Id;
            }

            return Json(new[] { town }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditTown([DataSourceRequest] DataSourceRequest request, AdminTowns town)
        {
            var existingTown = this.Data.Towns.GetById(town.Id);

            if (town != null && ModelState.IsValid)
            {
                existingTown.Name = town.Name;
                existingTown.AreaId = town.AreaId;               

                this.Data.SaveChanges();
            }

            return Json((new[] { town }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTown([DataSourceRequest] DataSourceRequest request, AdminTowns town)
        {
            var existingTown = this.Data.Towns.GetById(town.Id);

            this.Data.Towns.Delete(existingTown);
            this.Data.SaveChanges();

            return Json(new[] { town }, JsonRequestBehavior.AllowGet);
        }
	}
}