using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamScheduler.Models
{
    public class LocationList
    {
        public Locations Locations { get; set; }
        public List<Locations> AllLocations { get; set; }
    }
}