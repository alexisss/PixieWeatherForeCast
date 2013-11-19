using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class ExtendedUser : User
    {
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string PictureUrl { get; set; }
        public virtual ICollection<Forecast> Forecasts { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public ExtendedUser()
            :base()
        {
            this.Forecasts = new HashSet<Forecast>();
            this.Votes = new HashSet<Vote>();
        }
    }
}
