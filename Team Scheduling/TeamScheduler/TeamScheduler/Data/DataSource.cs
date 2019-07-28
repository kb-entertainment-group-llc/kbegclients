using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamScheduler.Models;
using MySql.Data.MySqlClient;

namespace TeamScheduler.Data
{
    public class DataSource 
    {
        string connectionString = "Server = localhost; Port=3306; Database = TeamScheduling; Uid = root; Pwd = !QAZ2wsx;";
        public bool ValidateBeforeCreateSchedule(Scheduler Sch)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "Select 1 from Scheduler where Age='" + Sch.AgeDivision + "'";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
               if (rdr.Read())
                {
                    rdr.Close();
                    return false;
                }
               rdr.Close();
                mCn.Close();
                return true;
            }
        }
        public void CreateSchedule(Scheduler Sch)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = @"insert into Scheduler (NumberOfGames,ScheduleType,Age,StartDate,EndDate,BracketRule,StartTime,EndTime,TimeBetweenGames, 
                         Monday,Tuesday,Wednasday,Thursday,Friday,Saturday,Sunday) values 
                        (" + Sch.NumberOfGames + ",'" + Sch.ScheduleType + "','" + Sch.AgeDivision + "','" + Sch.StartDate.Value.ToString("yyyy/MM/dd") + "','" + Sch.EndDate.Value.ToString("yyyy/MM/dd") + "','" + Sch.BracketRules + "','" + Sch.FromTime + "','" + Sch.EndTime + "','" + Sch.TimeBetweenGames + "'," + Sch.Monday + "," + Sch.Tuesday + "," + Sch.Wednasday + "," + Sch.Thursday + "," + Sch.Friday + "," + Sch.Saturday + "," + Sch.Sunday + ")";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();
                long ScheduleId = cmd.LastInsertedId;
                for (int i = 0; i < Sch.SelectedLocationIds.Count(); i++)
                {
                    sql = "insert into ScheduleLocations(scheduleId,LocationId) values(" + ScheduleId + ", " + Sch.SelectedLocationIds.ToList()[i] + ")";
                    cmd = new MySqlCommand(sql, mCn);
                    cmd.ExecuteNonQuery();
                }
                
                mCn.Close();
            }
        }

        public void CreateLocation(Locations Loc)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = @"insert into Locations (LocationName,StreetAddress,City,State,Zip) values ('" + Loc.LocationName + "','" + Loc.StreetAddress + "','" + Loc.City + "','" + Loc.State + "','" + Loc.Zip + "')";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateLocation(Locations Loc)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = @"update Locations set LocationName = '" + Loc.LocationName + "',StreetAddress='" + Loc.StreetAddress + "',City='" + Loc.City + "',State='" + Loc.State + "',Zip='" + Loc.Zip + "' where locationId=" + Loc.LocationId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSchedule(Scheduler Sch)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = @"update Scheduler set NumberOfGames=" + Sch.NumberOfGames + ",ScheduleType='" + Sch.ScheduleType + "',Age='" + Sch.AgeDivision + "',StartDate='" + Sch.StartDate.Value.ToString("yyyy/MM/dd") + "',EndDate='" + Sch.EndDate.Value.ToString("yyyy/MM/dd") + "',BracketRule='" + Sch.BracketRules + "',StartTime=" + Sch.FromTime + ",EndTime=" + Sch.EndTime + ",TimeBetweenGames='" + Sch.TimeBetweenGames + "',Monday=" + Sch.Monday + ",Tuesday=" + Sch.Tuesday + ",Wednasday=" + Sch.Wednasday + ",Thursday=" + Sch.Thursday + ",Friday=" + Sch.Friday + ",Saturday=" + Sch.Saturday + ",Sunday=" + Sch.Sunday + " where scheduleid = " + Sch.ScheduleId;  
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();

                sql = @"delete from ScheduleLocations where scheduleid = " + Sch.ScheduleId;
                cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < Sch.SelectedLocationIds.Count(); i++)
                {
                    sql = "insert into ScheduleLocations(scheduleId,LocationId) values(" + Sch.ScheduleId + ", " + Sch.SelectedLocationIds.ToList()[i] + ")";
                    cmd = new MySqlCommand(sql, mCn);
                    cmd.ExecuteNonQuery();
                }
                mCn.Close();
            }
        }
        public List<Scheduler> GetAllSchedules()
        {
            List<Scheduler> Model = new List<Scheduler>();
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select * from Scheduler";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Scheduler sch = new Scheduler { ScheduleId = (int)rdr["ScheduleId"], AgeDivision = rdr["Age"].ToString(),ScheduleType = rdr["ScheduleType"].ToString(), NumberOfGames = (int)rdr["NumberOfGames"], StartDate = (DateTime)rdr["StartDate"], EndDate = (DateTime)rdr["EndDate"] };
                    Model.Add(sch);
                }
                mCn.Close();
                return Model;
            }
        }

        public List<Locations> GetAllLocations()
        {
            List<Locations> Model = new List<Locations>();
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select * from Locations";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Locations sch = new Locations { LocationId = (int)rdr["LocationId"], LocationName = rdr["LocationName"].ToString() };
                    Model.Add(sch);
                }
                mCn.Close();
                return Model;
            }
        }

        public Scheduler GetScheduleById(int ScheduleId)
        {
            
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select * from Scheduler where scheduleid=" + ScheduleId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    Scheduler Model = new Scheduler { ScheduleId = (int)rdr["ScheduleId"], ScheduleType = rdr["ScheduleType"].ToString(), NumberOfGames = (int)rdr["NumberOfGames"], StartDate = ((DateTime)rdr["StartDate"]), EndDate = (DateTime)rdr["EndDate"], AgeDivision = (string)rdr["Age"], BracketRules = (string)rdr["BracketRule"], EndTime = (TimeSpan)rdr["EndTime"], FromTime = (TimeSpan)rdr["StartTime"], TimeBetweenGames = (int)rdr["TimeBetweenGames"], Sunday = (bool)rdr["Sunday"], Monday = (bool)rdr["Monday"], Tuesday = (bool)rdr["Tuesday"], Wednasday = (bool)rdr["Wednasday"], Thursday = (bool)rdr["Thursday"], Friday = (bool)rdr["Friday"], Saturday = (bool)rdr["Saturday"]};
                    rdr.Close();
                    mCn.Close();
                     return Model;
                }
                return new Scheduler();
            }
        }

        public List<string> GetLocationsByScheduleId(int ScheduleId)
        {

            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select LocationId from ScheduleLocations where scheduleid=" + ScheduleId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<string> Locations = new List<string>();
                while (rdr.Read()) 
                {
                    Locations.Add(Convert.ToString(rdr["LocationId"]));
                }
                return Locations;
            }
        }

        public Locations GetLocationById(int LocationId)
        {

            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select * from Locations where LocationId=" + LocationId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    Locations Model = new Locations { LocationId = (int)rdr["LocationId"], LocationName = rdr["LocationName"].ToString(), StreetAddress = Convert.ToString(rdr["StreetAddress"]), City = Convert.ToString(rdr["City"]), State = Convert.ToString(rdr["State"]), Zip = Convert.ToString(rdr["Zip"]) };
                    rdr.Close();
                    mCn.Close();
                    return Model;
                }
                return new Locations();
            }
        }
        //public List<Locations> GetAllLocations()
        //{
        //    List<Locations> Model = new List<Locations>();
        //    using (MySqlConnection mCn = new MySqlConnection(connectionString))
        //    {
        //        mCn.Open();
        //        string sql = "select * from Locations";
        //        MySqlCommand cmd = new MySqlCommand(sql, mCn);
        //        MySqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            Locations sch = new Locations { LocationId = (int)rdr["LocationId"], LocationName = (string)rdr["LocationName"] };
        //            Model.Add(sch);
        //        }
        //        mCn.Close();
        //        return Model;
        //    }
        //}
    }
}