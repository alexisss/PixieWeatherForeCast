using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Mvc.Controllers;

namespace WeatherForecast.Mvc.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminsController : BaseController
    {
       
	}
}