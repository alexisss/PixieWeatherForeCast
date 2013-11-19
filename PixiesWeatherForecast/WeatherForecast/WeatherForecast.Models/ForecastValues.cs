using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class ForecastValues
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ForecastId { get; set; }

        public virtual Forecast Forecast { get; set; }

        [Required]
        [Range(-100,+100)]
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
