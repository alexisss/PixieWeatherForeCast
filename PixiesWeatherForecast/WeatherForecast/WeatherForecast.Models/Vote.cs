using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Value { get; set; }

        public string AuthorId { get; set; }

        public virtual ExtendedUser Author { get; set; }

        [Required]
        public int ForecastId { get; set; }

        public virtual Forecast Forecast { get; set; }
    }
}
