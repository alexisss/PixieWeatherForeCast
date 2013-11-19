namespace WeatherForecast.Data
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;
    using WeatherForecast.Models;

    public interface IUowData : IDisposable
    {
        IRepository<Area> Areas { get; }

        IRepository<Town> Towns{ get; }

        IRepository<ExtendedUser> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Forecast> Forecasts { get; }
        IRepository<ForecastValues> ForecastValues { get; }
        IRepository<Vote> Votes { get; }
        int SaveChanges();
    }
}
