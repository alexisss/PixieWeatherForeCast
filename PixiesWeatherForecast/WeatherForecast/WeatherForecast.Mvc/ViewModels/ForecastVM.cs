using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast.Mvc.ViewModels
{
    public class ForecastVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TownId { get; set; }

        public string AreaName { get; set; }

        public string TownName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
        public virtual IEnumerable<ForecastValuesVM> Values { get; set; }

        public VoteVM Vote { get; set; }
    }
}