using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherForecast.Mvc.ViewModels
{
    public class VoteVM
    {
        public int Id { get; set; }
        public int ForecastId { get; set; }
        public int VotesCount { get; set; }
        public bool CanVoteUp { get; set; }
        public bool CanVoteDown { get; set; }
    }
}