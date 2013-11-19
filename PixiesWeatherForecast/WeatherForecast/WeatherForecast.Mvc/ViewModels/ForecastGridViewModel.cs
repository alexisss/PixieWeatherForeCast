using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WeatherForecast.Models;
using System.Linq.Expressions;

namespace WeatherForecast.Mvc.Models
{
    public class ForecastGridViewModel
    {
         public static Expression<Func<Forecast, ForecastGridViewModel>> FromForecast
        {
            get
            {
                return forecast => new ForecastGridViewModel
                {
                    TownId = forecast.TownId,
                    Name = forecast.Town.Name,
                    Weatherman = forecast.Author.FirstName + " "+ forecast.Author.LastName,
                    Date = forecast.Date
                };
            }
        }

        public int TownId { get; set; }

        public string Name { get; set; }

        public string Weatherman { get; set; }

        public DateTime Date { get; set; }
    }
}