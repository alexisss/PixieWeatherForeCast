using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Models
{
    public class ExtendedAuthorViewModel : AuthorViewModel
    {
        const string DefaultProfilePictureUrl = "../img/Account/default.jpg";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Descripiton { get; set; }

        public bool CanEdit { get; set; }

        public bool IsAuthor { get; set; }

        public IEnumerable<ForecastViewModel> Forecasts { get; set; }

        public ExtendedAuthorViewModel()
        {
            this.Forecasts = new HashSet<ForecastViewModel>();
        }

        public static Expression<Func<ExtendedUser, ExtendedAuthorViewModel>> ExtendedFromUser
        {
            get
            {
                return user => new ExtendedAuthorViewModel
                {
                    AuthorId = user.Id,
                    Rating = user.Forecasts.Average(f => f.Votes.Sum(vote => vote.Value)) == null ? 0 : user.Forecasts.Average(f => f.Votes.Sum(vote => vote.Value)),
                    UserName = user.UserName,
                    ProfilePictureUrl = string.IsNullOrEmpty(user.PictureUrl) ? DefaultProfilePictureUrl : user.PictureUrl,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Descripiton = user.Description,
                    IsAuthor = user.Roles.Any(r=>r.Role.Name == "Author")
                };
            }
        }
    }
}