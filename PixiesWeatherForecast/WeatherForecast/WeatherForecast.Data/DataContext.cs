using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WeatherForecast.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WeatherForecast.Data
{
    public class DataContext : IdentityDbContextWithCustomUser<ExtendedUser>
    {
        public IDbSet<Area> Areas { get; set; }

        public IDbSet<Town> Towns { get; set; }
        public IDbSet<Forecast> Forecasts { get; set; }
        public IDbSet<ForecastValues> ForecastValues { get; set; }
        public IDbSet<Vote> Votes { get; set; }

    }
}
