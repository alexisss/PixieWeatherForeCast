using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Areas.Administration.ViewModels
{
    public class AdminAreas
    {
        public static Expression<Func<Area, AdminAreas>> FromArea
        {
            get
            {
                return area => new AdminAreas
                {
                    Id = area.Id,
                    Name = area.Name                   
                };
            }
        }

        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
}