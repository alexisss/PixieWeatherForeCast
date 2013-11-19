using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class Town
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int AreaId { get; set; }

        public virtual Area Area { get; set; }

        public virtual ICollection<Forecast> Forecasts { get; set; }

        public Town()
        {
            this.Forecasts = new HashSet<Forecast>();
        }
    }
}
