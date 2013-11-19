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
    public class AdminExtendedUser
    {
        public static Expression<Func<ExtendedUser, AdminExtendedUser>> FromExtendedUser
        {
            get
            {
                return user => new AdminExtendedUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Description = user.Description,
                    PictureUrl = user.PictureUrl
                    
                };
            }
        }
        
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
       
        public string UserName { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
              
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        public string PictureUrl { get; set; }
    }
}