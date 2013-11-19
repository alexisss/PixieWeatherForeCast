using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;

namespace WeatherForecast.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public BaseController() :this(new UowData())
        {           
        }

        protected IUowData Data { get; set; }
    }
}