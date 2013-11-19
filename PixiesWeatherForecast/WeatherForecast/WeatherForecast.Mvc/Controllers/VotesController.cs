using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WeatherForecast.Mvc.ViewModels;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Controllers
{
    public class VotesController : BaseController
    {
        //
        // GET: /Votes/
        [Authorize]
        public ActionResult Vote(int id, int value)
        {
            var userId = User.Identity.GetUserId();
            var forecastDb = this.Data.Forecasts.GetById(id);

            Vote vote = new Vote()
            {
                AuthorId = userId,
                Forecast = forecastDb,
                Value = value
            };

            this.Data.Votes.Add(vote);
            this.Data.SaveChanges();

            VoteVM model = new VoteVM
            {
                VotesCount = forecastDb.Votes.Sum(x => x.Value),
            };

            return PartialView(model);
        }
    }
}