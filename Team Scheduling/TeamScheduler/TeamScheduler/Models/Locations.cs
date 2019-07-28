using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TeamScheduler.Models
{
    public class Locations
    {
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Location Name is required")]
        [DisplayName("Location Name")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Street Address is required")]
        [DisplayName("Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "State")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [DisplayName("Zip Code")]
        public string Zip { get; set; }
    }
}