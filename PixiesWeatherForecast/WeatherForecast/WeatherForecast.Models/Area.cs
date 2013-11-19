using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Town> Towns { get; set; }

        public Area()
        {
            this.Towns = new HashSet<Town>();
        }
    }
}
