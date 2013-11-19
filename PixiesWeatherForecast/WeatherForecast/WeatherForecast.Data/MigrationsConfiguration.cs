using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using WeatherForecast.Models;

namespace WeatherForecast.Data
{
    public class MigrationsConfiguration : DbMigrationsConfiguration<DataContext>
    {
        public MigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context)
        {
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");

            if (adminRole == null)
            {
                string[] areas = new string[] {
                    "Vidin", "Montana", "Vratza", "Pleven", "Lovech", "Veliko Tarnovo", "Gabrovo", "Ruse", "Razgrad",
                    "Targovishte", "Shumen", "Silistra", "Dobrich", "Varna", "Bourgas", "Sliven", "Yambol", "Stara Zagora",
                    "Plovdiv", "Pazardjik", "Smolyan", "Haskovo", "Kardjali", "Sofia", "Pernik", "Kyustendil", "Blagoevgrad"
                };

                foreach (var area in areas)
                {
                    Area newArea = new Area
                    {
                        Name = area
                    };

                    Town town = new Town
                    {
                        Name = area,
                        Area = newArea
                    };

                    context.Areas.Add(newArea);
                    context.Towns.Add(town);
                }

                Role admin = new Role
                {
                    Name = "Admin"
                };

                Role author = new Role
                {
                    Name = "Author"
                };

                context.Roles.Add(admin);
                context.Roles.Add(author);

                context.SaveChanges();                
            }

            base.Seed(context);
        }
    }
}