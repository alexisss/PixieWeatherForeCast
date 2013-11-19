using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Areas.Administration.ViewModels
{
    public class AdminTowns
    {
        public static Expression<Func<Town, AdminTowns>> FromTown
        {
            get
            {
                return town => new AdminTowns
                {
                    Id = town.Id,
                    Name = town.Name,
                    AreaId = town.Area.Id,
                    AreaName = town.Area.Name
                };
            }
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }       
        public string Name { get; set; }        
        public int AreaId { get; set; }
        
        [HiddenInput(DisplayValue=false)]
        public string AreaName { get; set; }
        
    }
}