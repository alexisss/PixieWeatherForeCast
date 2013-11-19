using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Models;
using Kendo.Mvc.Extensions;
using WeatherForecast.Mvc.ViewModels;
using Microsoft.AspNet.Identity;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.Controllers
{
    public class ForecastsController : BaseController
    {
        const string DateFormat = "MM/dd/yyyy";

        public ActionResult CreateForecast([DataSourceRequest] DataSourceRequest request, ForecastViewModel forecast,
                                            TownViewModel town, DateTime date)
        {
            if (!User.IsInRole("Author"))
            {
                throw new Exception("Current user is not an author!");
            }

            if (ModelState.IsValid)
            {

                Town townDb;
                if (string.IsNullOrEmpty(town.Name))
                {
                    townDb = this.Data.Towns.All().FirstOrDefault();
                }
                else
                {
                    townDb = this.Data.Towns.GetById(town.TownId);
                }
                Forecast newForecast = new Forecast()
                {
                    AuthorId = User.Identity.GetUserId(),
                    Date = date.Date,
                    Town = townDb,

                };
                var dayParts = Enum.GetValues(typeof(DayPart));

                foreach (var dayPart in dayParts)
                {
                    ForecastValues values = new ForecastValues()
                    {
                        DayPart = (DayPart)dayPart,
                        Pressure = 700
                    };

                    newForecast.Values.Add(values);
                }

                this.Data.Forecasts.Add(newForecast);
                this.Data.SaveChanges();

                forecast = GetForecastVMFromDb(newForecast.Id);
            }

            return Json((new[] { forecast }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllTowns()
        {
            var towns = this.Data.Towns.All().Select(TownViewModel.FromTown);

            return Json(towns, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetForecasts([DataSourceRequest]DataSourceRequest request, string Username)
        {
            var result = this.Data.Forecasts.All()
                .Where(x => x.Author.UserName == Username)
                .Select(ForecastViewModel.FromForecast);


            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Author")]
        public JsonResult UpdateForecast([DataSourceRequest] DataSourceRequest request, ForecastViewModel forecast,
            TownViewModel town, DateTime date)
        {
            var forecastCreatorUserName = this.Data.Forecasts.GetById(forecast.ForecastId).AuthorId;

            if (!((User.IsInRole("Author")
                && User.Identity.GetUserId() == forecastCreatorUserName)
                || User.IsInRole("Admin")))
            {
                throw new Exception("Current user does not have the rights to update forecast!");
            }

            var existingForecast = this.Data.Forecasts.GetById(forecast.ForecastId);

            if (existingForecast != null && ModelState.IsValid)
            {
                existingForecast.TownId = town.TownId;
                existingForecast.Date = date.Date;
                Data.SaveChanges();
                forecast = GetForecastVMFromDb(existingForecast.Id);
            }

            return Json((new[] { forecast }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        private ForecastViewModel GetForecastVMFromDb(int forecastId)
        {
            ForecastViewModel forecast = new ForecastViewModel();
            var savedForecast = this.Data.Forecasts.GetById(forecastId);
            forecast.Date = savedForecast.Date;
            forecast.TownName = savedForecast.Town.Name;
            forecast.TownId = savedForecast.TownId;
            forecast.ForecastId = savedForecast.Id;
            return forecast;
        }

        [Authorize(Roles = "Admin, Author")]
        public JsonResult DestroyForecast([DataSourceRequest] DataSourceRequest request, ForecastViewModel forecast)
        {
            var forecastCreatorUserName = this.Data.Forecasts.GetById(forecast.ForecastId).AuthorId;

            if (!((User.IsInRole("Author")
                && User.Identity.GetUserId() == forecastCreatorUserName)
                || User.IsInRole("Admin")))
            {
                throw new Exception("Current user does not have the rights to delete forecast!");
            }

            var existingForecast = Data.Forecasts.GetById(forecast.ForecastId);

            if (existingForecast != null)
            {
                foreach (var forecastValue in existingForecast.Values.ToList())
                {
                    this.Data.ForecastValues.Delete(forecastValue.Id);
                }

                foreach (var vote in existingForecast.Votes.ToList())
                {
                    this.Data.Votes.Delete(vote.Id);
                }

                this.Data.Forecasts.Delete(existingForecast.Id);

                this.Data.SaveChanges();
            }

            return Json((new[] { forecast }.ToDataSourceResult(request, ModelState)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int id)
        {
            var forecastDb = this.Data.Forecasts.GetById(id);
            var userId = User.Identity.GetUserId();

            if (forecastDb == null)
            {
                throw new ArgumentException("No such weather forecast.");
            }

            ForecastVM model = this.Data.Forecasts.All().Where(f => f.Id == id).Select(forecast => new ForecastVM
            {
                Date = forecast.Date,
                Id = forecast.Id,
                AreaName = forecast.Town.Area.Name,
                TownName = forecast.Town.Name,
                AuthorId = forecast.AuthorId,
                AuthorName = forecast.Author.UserName,
                Vote = new VoteVM
                {
                    CanVoteDown = !forecast.Votes.Any(v => v.Value == -1 && v.AuthorId == userId),
                    CanVoteUp = !forecast.Votes.Any(v => v.Value == 1 && v.AuthorId == userId),
                    ForecastId = forecast.Id,
                    VotesCount = forecast.Votes.Sum(v => v.Value) == null ? 0 : forecast.Votes.Sum(v => v.Value)
                },
                Values = forecast.Values.Select(f => new ForecastValuesVM
                {
                    ForecastId = f.ForecastId,
                    DayPart = f.DayPart,
                    Id = f.Id,
                    Pressure = f.Pressure,
                    RainProbability = f.RainProbability,
                    State = f.State,
                    Temperature = f.Temperature,
                    WindSpeed = f.WindSpeed,
                }),
            }).FirstOrDefault();

            return View(model);
        }

        [Authorize(Roles = "Admin, Author")]
        public ActionResult Edit(int id)
        {
            var forecast = this.Data.Forecasts.GetById(id);

            if (forecast == null)
            {
                throw new ArgumentException("No such weather forecast.");
            }

            ForecastVM model = new ForecastVM
            {
                Date = forecast.Date,
                Id = forecast.Id,
                TownId = forecast.TownId,
                Values = forecast.Values.Select(f => new ForecastValuesVM
                {
                    ForecastId = f.ForecastId,
                    DayPart = f.DayPart,
                    Id = f.Id,
                    Pressure = f.Pressure,
                    RainProbability = f.RainProbability,
                    State = f.State,
                    Temperature = f.Temperature,
                    WindSpeed = f.WindSpeed,
                }),
            };

            var townsList = new SelectList(this.Data.Towns.All().OrderBy(t => t.Name).ToList(), "Id", "Name", model.TownId);

            ViewBag.TownsList = townsList;
            return View(model);
        }

        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        public ActionResult UpdateForecatst(int id, int townId, DateTime date)
        {
            var forecast = this.Data.Forecasts.GetById(id);
            var town = this.Data.Towns.GetById(townId);

            if (forecast == null)
            {
                throw new ArgumentException("No such weather forecast.");
            }

            if (town == null)
            {
                throw new ArgumentException("No such town.");
            }

            if (!(DateTime.Today <= date && date <= DateTime.Today.AddDays(11)))
            {
                throw new ArgumentException("Invalid date.");
            }

            forecast.Town = town;
            forecast.TownId = townId;
            forecast.Date = date;
            this.Data.SaveChanges();

            return Content("");
        }

        [Authorize(Roles = "Admin, Author")]
        public ActionResult UpdateValues([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<ForecastValuesVM> values)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentForecastId = values.FirstOrDefault().ForecastId;
            var currentForecast = this.Data.Forecasts.GetById(currentForecastId);

            if (currentForecast == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest,
                    "Invalid request. Forecasts values points to non existing forecast.");
            }

            if (!(User.IsInRole("Admin") ||
                (User.IsInRole("Author") && currentForecast.Author.Id != currentUserId)))
            {
                string message = string.Format(
                    "Invalid request. Current user does not have rights to edit forecast with id = {0}",
                    currentForecast.Id);

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, message);
            }

            if (!this.AreForecastsValuesValid(values))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest,
                    "Invalid request. Forecasts values points to different forecast.");
            }


            if (ModelState.IsValid)
            {
                foreach (var value in values)
                {
                    var dbValue = this.Data.ForecastValues.GetById(value.Id);
                    if (dbValue != null)
                    {
                        dbValue.Pressure = value.Pressure;
                        dbValue.RainProbability = value.RainProbability;
                        dbValue.State = value.State;
                        dbValue.Temperature = value.Temperature;
                        dbValue.WindSpeed = value.WindSpeed;
                    }
                }
                this.Data.SaveChanges();

                return Json(new { });
            }
            else
            {
                return Json(ModelState.ToDataSourceResult(request));
            }
        }

        private bool AreForecastsValuesValid(IEnumerable<ForecastValuesVM> values)
        {
            var forecastId = values.FirstOrDefault().ForecastId;

            foreach (var value in values)
            {
                if (forecastId != value.ForecastId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}