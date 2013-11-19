namespace WeatherForecast.Data
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using WeatherForecast.Models;

    public class UowData : IUowData
    {
        private readonly DataContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UowData()
            : this(new DataContext())
        {
        }

        public UowData(DataContext context)
        {
            this.context = context;
        }

        public IRepository<Area> Areas
        {
            get
            {
                return this.GetRepository<Area>();
            }
        }

        public IRepository<Town> Towns
        {
            get
            {
                return this.GetRepository<Town>();
            }
        }

        public IRepository<Forecast> Forecasts
        {
            get
            {
                return this.GetRepository<Forecast>();
            }
        }

        public IRepository<ForecastValues> ForecastValues
        {
            get
            {
                return this.GetRepository<ForecastValues>();
            }
        }

        public IRepository<ExtendedUser> Users
        {
            get
            {
                return this.GetRepository<ExtendedUser>();
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                return this.GetRepository<Role>();
            }
        }
        public IRepository<Vote> Votes
        {
            get
            {
                return this.GetRepository<Vote>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
