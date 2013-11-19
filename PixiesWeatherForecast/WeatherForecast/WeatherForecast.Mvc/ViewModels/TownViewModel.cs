using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Models
{
    public class TownViewModel
    {
        public int TownId{ get; set; }

        public string Name { get; set; }

        public static Expression<Func<Town, TownViewModel>> FromTown
        {
            get
            {
                return town => new TownViewModel
                {
                    Name = town.Name,
                    TownId = town.Id
                };
            }
        }
    }
}