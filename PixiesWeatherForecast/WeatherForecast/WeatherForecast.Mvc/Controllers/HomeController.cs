using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Models;
using Kendo.Mvc.Extensions;

namespace WeatherForecast.Mvc.Controllers
{
    public class HomeController : BaseController
    {
         public HomeController(IUowData data)
             :base(data)
        {
        }

         public HomeController()
             : base()
        {           
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetAreas()
        {
            var areas = this.Data.Areas.All().Select(x => new { Id = x.Id, Name = x.Name });
            return Json(areas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCascadeTowns(string areas)
        {
            int areaId = Convert.ToInt32(areas);
            var towns = this.Data.Towns.All();

            if (!string.IsNullOrEmpty(areas))
            {
                towns = towns.Where( x => x.Area.Id == areaId);   
            }

            return Json(towns.Select(y => new { Id = y.Id, Name = y.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadForecasts([DataSourceRequest]DataSourceRequest request,string TownId, string Date)
        {
            //IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            int townId = Convert.ToInt32(TownId);
            DateTime day = DateTime.Parse(Date);
            
            
            var forecasts = this.Data.Forecasts.All()
                .Where(x => x.TownId == townId && x.Date == day)
                .Select(ForecastGridViewModel.FromForecast );
            
            return Json(forecasts.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ReadForecasts1([DataSourceRequest]DataSourceRequest request, string TownId, string Date)
        //{
        //    IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
        //    int townId = Convert.ToInt32(TownId);
        //    DateTime day = DateTime.Parse(Date, culture, System.Globalization.DateTimeStyles.AssumeLocal);


        //    var forecasts = this.Data.Forecasts.All().Where(x => x.TownId == townId && x.Date == day).Select(ForecastGridViewModel.FromForecast);
        //    return Json(forecasts.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
    }
}