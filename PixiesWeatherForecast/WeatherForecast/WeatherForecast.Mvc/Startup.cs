using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeatherForecast.Mvc.Startup))]
namespace WeatherForecast.Mvc
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
