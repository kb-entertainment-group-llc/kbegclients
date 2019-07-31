using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TeamScheduler.Data;
using System.Web.Mvc;
namespace TeamScheduler.Models
{
    public class Scheduler
    {
        public IEnumerable<string> SelectedLocationIds { get; set; }
        public int ScheduleId { get; set; }
        [Required(ErrorMessage = "Schedule type is required")]
        public string ScheduleType { get; set; }

        [Required(ErrorMessage = "Number of Games is required")]
        [DisplayName("Number of Games")]
        public int NumberOfGames { get; set; }

        [Required(ErrorMessage = "Age Division is required")]
        [DisplayName("Age Division")]
        public string AgeDivision { get; set; }

        [Required(ErrorMessage = "Bracket Rules")]
        [DisplayName("Bracket Rules(Playoff Only)")]
        public string BracketRules { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "From Time")]
        
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan FromTime { get; set; }

        [Required(ErrorMessage = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Time Between Games is required")]
        [DisplayName("Time Between Games")]
        public int TimeBetweenGames { get; set; }
        [DisplayName("Select Location")]
        public int Location { get; set; }
        [DisplayName("Enable Multiple Locations")]
        public bool MultipleLocation { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednasday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public bool IsTournamentScheduleCreated { get; set; }

        public System.Web.Mvc.SelectList ScheduleTypeCollection = new System.Web.Mvc.SelectList(new[]
        {
            
            new System.Web.Mvc.SelectListItem { Text = "Regular Season", Value = "Regular Season", Selected=true},
            new System.Web.Mvc.SelectListItem  {Text = "Playoffs", Value = "Playoffs" },
            new System.Web.Mvc.SelectListItem { Text = "Off Season", Value = "Off Season" },
        }, "Value", "Text");

        public System.Web.Mvc.SelectList AgeDivisionCollection = new System.Web.Mvc.SelectList(new[]
        {
            
            new System.Web.Mvc.SelectListItem { Text = "9U Minor", Value = "9U Minor", Selected=true},
            new System.Web.Mvc.SelectListItem  {Text = "9U Major", Value = "9U Major" },
            new System.Web.Mvc.SelectListItem  {Text = "10U Minor", Value = "10U Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "10U Major", Value = "10U Major" },
            new System.Web.Mvc.SelectListItem  {Text = "11 Minor", Value = "11 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "11 Major", Value = "11 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "12 Minor", Value = "12 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "12 Major", Value = "12 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "13 Minor", Value = "13 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "13 Major", Value = "13 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "14 Minor", Value = "14 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "14 Major", Value = "14 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "15 Minor", Value = "15 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "15 Major", Value = "15 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "16 Minor", Value = "16 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "16 Major", Value = "16 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "17 Minor", Value = "17 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "17 Major", Value = "17 Major" },
            new System.Web.Mvc.SelectListItem  {Text = "18 Minor", Value = "18 Minor" },
            new System.Web.Mvc.SelectListItem  {Text = "18 Major", Value = "18 Major" },
            }, "Value", "Text");

        public System.Web.Mvc.SelectList BracketRulesCollection = new System.Web.Mvc.SelectList(new[]
        {
            
            new System.Web.Mvc.SelectListItem { Text = "Single Elimination", Value = "Single Elimination", Selected=true},
            new System.Web.Mvc.SelectListItem  {Text = "Manual", Value = "Manual" },
            new System.Web.Mvc.SelectListItem { Text = "N/A", Value = "N/A" },
        }, "Value", "Text");

        public IEnumerable<SelectListItem> LocationsCollection
        {
            get { return GetRoles(); }
        }

        public IEnumerable<SelectListItem> GetRoles()
        {
            List<Locations> lst = new List<Locations>();
            DataSource ds = new DataSource();
            {
                
                lst = ds.GetAllLocations();
                //LocationsCollection=
            }
            
            var roles = lst
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.LocationId.ToString(),
                                    Text = x.LocationName
                                });

            return new SelectList(roles, "Value", "Text");
        }
        

    }

    public class AllSchedules
    {
        public List<Scheduler> Schedules { get; set; }
    }
}