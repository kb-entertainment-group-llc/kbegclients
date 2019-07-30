using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using TeamScheduler.Models;
using TeamScheduler.Data;
using System.Xml;
using System.Net;

namespace TeamScheduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Scheduler(String id = "")
        {
            ViewBag.Message = "Your schedule page.";
            Scheduler Model ;
            if (string.IsNullOrEmpty(id))
            {
                Model = new Scheduler();
                ViewData["Mode"] = "CreateSchedule";
            }
            else
            {
                DataSource ds = new DataSource();
                Model =ds.GetScheduleById(Convert.ToInt16(id));
                Model.SelectedLocationIds = ds.GetLocationsByScheduleId(Convert.ToInt16(id));
                ViewData["Mode"] = "UpdateSchedule";
            }
            return View(Model);
        }

        public ActionResult CreateTournamentSchedule(String id = "")
        {
            ViewBag.Message = "Your schedule page.";
            Scheduler Model;
            if (!string.IsNullOrEmpty(id))
            {
                DataSource ds = new DataSource();
                Model = ds.GetScheduleById(Convert.ToInt16(id));
                ds.CreateTournamentSchedule(Model);
                return View(Model);
            }
            return View();
        }

        public ActionResult AllSchedules()
        {
            ViewBag.Message = "Your schedule page.";

            DataSource ds = new DataSource();

            return View(ds.GetAllSchedules());
        }

        public ActionResult AddLocation(int id = 0)
        {
            ViewBag.Message = "Your schedule page.";

            DataSource ds = new DataSource();
            if (id < 1)
            {
                LocationList model = new LocationList { Locations = new Locations(), AllLocations = ds.GetAllLocations() };
                return View("Locations", model);
            }
            else
            {
                LocationList model = new LocationList { Locations = ds.GetLocationById(id), AllLocations = ds.GetAllLocations() };
                return View("Locations", model);
            }
        }

        [HttpPost]
        public ActionResult Create(Scheduler Sch)
        {
            //string requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?units=imperial&origins=15757 N 90th Pl,85260C&destinations=21001 N Tatum Blvd,85050&key=AIzaSyCG40GGV0La-19u8JxCMddds-fXc1iZFUA");

            //var client = new WebClient();
            //var content = client.DownloadString(requestUri);
            //XmlDocument doc = new XmlDocument();

            //doc.LoadXml(content);
            //XmlNode node = doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/distance/text");

            //var t = node.InnerXml;
            ModelState.Clear();
            DataSource ds = new DataSource();
            if (Sch.LocationsCollection ==null ||Sch.LocationsCollection.Count()==0)
            {
                ModelState.AddModelError("", "Please select Location");
                return View("Scheduler", Sch);
            }
            
            if (ViewData["Mode"]==null || Convert.ToString(ViewData["Mode"]) == "CreateSchedule")
            {
                if (!ds.ValidateBeforeCreateSchedule(Sch))
                {
                    ModelState.AddModelError("", "Age Division already exists");
                    return View("Scheduler", Sch);
                }
                if (!ds.IsDistanceBetweenLocationsOk(Sch.SelectedLocationIds.ToList()))
                {
                    ModelState.AddModelError("", "Distance between Locations are more than allowed limit");
                    return View("Scheduler", Sch);
                }
                ds.CreateSchedule(Sch);
            }
            else
            {
                ds.UpdateSchedule(Sch);
            }
            
        return RedirectToAction("AllSchedules", "Home");
            
        }

        [HttpPost]
        public ActionResult CreateLocation(LocationList model)
        {
            try
            {
                DataSource ds = new DataSource();
            {
                if (model.Locations.LocationId <= 0)
                {
                    ds.CreateLocation(model.Locations);
                }
                else
                {
                    ds.UpdateLocation(model.Locations);
                }
            }
            }
            catch (Exception ex)
            {
                string g = ex.Message;
            }
            
            return RedirectToAction("AllSchedules", "Home");
        }
    }
}
