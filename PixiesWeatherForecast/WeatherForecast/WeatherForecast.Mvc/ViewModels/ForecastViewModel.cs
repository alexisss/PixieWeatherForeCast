using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Models
{
    public class ForecastViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ForecastId { get; set; }

        //public TownViewModel Town { get; set; }

        public string TownName { get; set; }

        public int TownId { get; set; }
        public DateTime Date { get; set; }


        public static Expression<Func<Forecast, ForecastViewModel>> FromForecast
        {
            get
            {
                return forecast => new ForecastViewModel
                {
                    Date = forecast.Date,
                    ForecastId = forecast.Id,
                    //Town = new TownViewModel() { TownId = forecast.TownId, Name = forecast.Town.Name },
                    TownId = forecast.TownId,
                    TownName = forecast.Town.Name
                };
            }
        }
    }
}
