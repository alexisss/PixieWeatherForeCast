using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherForecast.Mvc.Areas.Administration.Controllers
{
     //[Authorize(Roles = "Admin")]
    public class StartController : AdminsController
    {       
        // GET: /Administration/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}