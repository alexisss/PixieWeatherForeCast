using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Models
{
    public class AuthorViewModel
    {
        const string DefaultProfilePictureUrl = "../img/Account/default.jpg";
        public string AuthorId { get; set; }

        public string UserName { get; set; }

        public double Rating { get; set; }

        public string ProfilePictureUrl { get; set; }

        public static Expression<Func<ExtendedUser, AuthorViewModel>> FromUser
        {
            get
            {
                return user => new AuthorViewModel
                {
                    AuthorId = user.Id,
                    Rating = user.Forecasts.Average(f => f.Votes.Sum(vote => vote.Value)) == null ? 0 : user.Forecasts.Average(f => f.Votes.Sum(vote => vote.Value)),
                    UserName = user.UserName,
                    ProfilePictureUrl = string.IsNullOrEmpty(user.PictureUrl) ? DefaultProfilePictureUrl : user.PictureUrl
                };
            }
        }

    }
}