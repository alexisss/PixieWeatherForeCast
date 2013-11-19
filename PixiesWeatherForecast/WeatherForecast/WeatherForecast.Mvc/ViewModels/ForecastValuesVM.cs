using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.ViewModels
{
    public class ForecastValuesVM
    {
        public static Expression<Func<ForecastValues, ForecastValuesVM>> FromForecastValues
        {
            get
            {
                return f => new ForecastValuesVM
                {
                    DayPart = f.DayPart,
                    Id = f.Id,
                    Pressure = f.Pressure,
                    RainProbability = f.RainProbability,
                    State = f.State,
                    Temperature = f.Temperature,
                    WindSpeed = f.WindSpeed
                };
            }
        }
        public int Id { get; set; }

        [Required]
        public int ForecastId { get; set; }

        [Required]
        [Range(-100, +100)]
        public decimal Temperature { get; set; }

        [Required]
        [Range(0, +300)]
        public decimal WindSpeed { get; set; }

        [Required]
        [Range(700, 2000)]
        public decimal Pressure { get; set; }

        [Required]
        [Range(0, 100)]
        public int RainProbability { get; set; }

        public DayPart DayPart { get; set; }

        public WeatherType State { get; set; }
    }
}