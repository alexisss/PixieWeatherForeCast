using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class Forecast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public virtual ExtendedUser Author { get; set; }
        public virtual ICollection<ForecastValues> Values { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

        public Forecast()
        {
            this.Values = new HashSet<ForecastValues>();
            this.Votes = new HashSet<Vote>();
        }
    }
}
