using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamScheduler.Models
{
    public class TournamentSchedule
    {
        public string TeamOne { get; set; }
        public string TeamTwo { get; set; }
        
        public string GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string Location { get; set; }
    }
}