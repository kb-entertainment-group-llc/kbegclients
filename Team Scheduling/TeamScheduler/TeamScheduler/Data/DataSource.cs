using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamScheduler.Models;
using MySql.Data.MySqlClient;
using System.Net;
using System.Xml;

namespace TeamScheduler.Data
{
    public class DataSource 
    {
        string connectionString = "Server = localhost; Port=3306; Database = TeamScheduling; Uid = root; Pwd = !QAZ2wsx;";
        List<string> gm = new List<string>();

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
                         Monday,Tuesday,Wednasday,Thursday,Friday,Saturday,Sunday,GameDuration) values 
                        (" + Sch.NumberOfGames + ",'" + Sch.ScheduleType + "','" + Sch.AgeDivision + "','" + Sch.StartDate.Value.ToString("yyyy/MM/dd") + "','" + Sch.EndDate.Value.ToString("yyyy/MM/dd") + "','" + Sch.BracketRules + "','" + Sch.FromTime + "','" + Sch.EndTime + "','" + Sch.TimeBetweenGames + "'," + Sch.Monday + "," + Sch.Tuesday + "," + Sch.Wednasday + "," + Sch.Thursday + "," + Sch.Friday + "," + Sch.Saturday + "," + Sch.Sunday + "," + Sch.GameDuration + ")";
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

        public void CreateTournamentSchedule(Scheduler Sch)
        {
           List<int> teams= GetTeamsInSchedule(Sch.ScheduleId);
           List<string> Locations = GetLocationsByScheduleId(Sch.ScheduleId);
           DateTime StartDate = Sch.StartDate.Value;
           DateTime EndDate = Sch.EndDate.Value;
           TimeSpan FromTime = Sch.FromTime;
           TimeSpan ToTime = Sch.EndTime;
           DateTime GameDate=StartDate;
           TimeSpan GameTime=FromTime;
           int TimeBetweenGames = Sch.TimeBetweenGames;
           int TotalTeams = teams.Count;
           // int TeamGames = teams.Count / 2;
           int counter=0;
           TimeSpan GameDuration = new TimeSpan(0, Sch.GameDuration, 0);
           TimeSpan DailyEndTime = ToTime.Subtract(GameDuration);
           int[] te = new int[TotalTeams-1];
           for (int i = 0; i < TotalTeams - 1;i++ )
           {
               te[i] = teams[i];
           }
            if (TotalTeams%2==0)
            {

            }
           DoLogic(TotalTeams);
           int cgame = 0;
            do
               {
                if (CanPlayGameOnDay(GameDate, Sch))
                   {
                       for (int i = 0; i < Locations.Count; i++)
                       {
                           GameTime = FromTime;
                           do
                           {
                               if (cgame >= gm.Count) break;   
                               string[] s = gm[cgame].Split(',');
                               ScehduleGame(Sch.ScheduleId, teams[Convert.ToInt16(s[0])-1], teams[Convert.ToInt16(s[1])-1], GameDate, GameTime, Convert.ToInt16(Locations[i]));
                               GameTime = GameTime.Add(GameDuration);
                               GameTime = GameTime.Add(new TimeSpan(0, TimeBetweenGames, 0));
                               counter = counter + 2;
                               cgame++;
                           } while (GameTime <= DailyEndTime);
                       }
                   }
                   GameDate = GameDate.AddDays(1);
               }
               while (GameDate <= EndDate);
           
        }

 

        private void DoLogic(int tot)
        {
            bool isOdd = false;
            int[] g = new int[tot];
            for (int i = 0; i < tot; i++)
            {
                g[i] = i + 1;
            }
            if (tot%2!=0)
            {
                isOdd = true;
                Array.Resize(ref  g, g.Length + 1);
                g[g.Length - 1] = -1;
                tot = tot + 1;
            }
            List<int> lst = g.ToList();
            lst.RemoveAt(0);
            for (int v = 0; v < lst.Count; v++)
            {
                int[] na = lst.ToArray();
                int[] a = new int[tot / 2];
                int[] b = new int[tot / 2];
                a[0] = g[0];
                for (int i = 1; i < tot / 2; i++)
                {
                    a[i] = na[i - 1];
                }

                int j = 0;
                int ci = tot / 2 - 1;
                for (int i = tot - 1; i >= tot / 2; i--)
                {
                    b[j] = na[i - 1];
                    ci--;
                    j++;
                }
                var tmp = lst[lst.Count - 1];
                lst.RemoveAt(lst.Count - 1);
                lst.Insert(0, tmp);
                Schedule(a, b);
            }
        }

        private void Schedule(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != -1 && b[i] != -1)
                {
                    gm.Add(a[i].ToString() + "," + b[i].ToString());
                }
            }
        }
        private void ScehduleGame(int ScheduleId,int TeamOne,int TeamTwo,DateTime GameDate,TimeSpan StartTime,int LocationId)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "insert into tournamentschedule(teamone, teamtwo, locationid, gamedate, starttime,scheduleid) values (" + TeamOne + "," + TeamTwo + "," + LocationId + ",'" + GameDate.ToString("yyyy/MM/dd") + "','" + StartTime + "'," + ScheduleId + ")";
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                cmd.ExecuteNonQuery();
                mCn.Close();
            }
        }
        bool CanPlayGameOnDay(DateTime GameDate,Scheduler Sch)
        {
            switch (GameDate.DayOfWeek)
            {
                case DayOfWeek.Sunday :
                    if (Sch.Sunday) return true;
                    break;
                case DayOfWeek.Monday:
                    if (Sch.Monday) return true;
                    break;
                case DayOfWeek.Tuesday:
                    if (Sch.Tuesday) return true;
                    break;
                case DayOfWeek.Wednesday:
                    if (Sch.Wednasday) return true;
                    break;
                case DayOfWeek.Thursday:
                    if (Sch.Thursday) return true;
                    break;
                case DayOfWeek.Friday:
                    if (Sch.Friday) return true;
                    break;
                case DayOfWeek.Saturday:
                    if (Sch.Saturday) return true;
                    break;
                default :
                    return false;
                    break;
            }

            return false;
        }
        public bool IsDistanceBetweenLocationsOk(List<string> Locations)
        {
            int LocationCount = Locations.Count();
            String[] Addresses = new String[LocationCount];
            for (int i = 0; i < LocationCount; i++)
            {
                var address = GetLocationById(Convert.ToInt16(Locations[i]));
                Addresses[i] = address.StreetAddress + "," + address.Zip;
            }
            int count = 0;
            
            do
            {
                for (int i = count; i < LocationCount-1; i++)
                {
                    double distanceBetweenLocations = GetDistance(Addresses[count], Addresses[i + 1]);
                    if (distanceBetweenLocations > 5) return false;
                }
                count++;

            } while (count < LocationCount - 1);
            return true;

        }

        double GetDistance(string AddressOne, string AddressTwo)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?units=imperial&origins=" + AddressOne + "&destinations=" + AddressTwo + "&key=AIzaSyCG40GGV0La-19u8JxCMddds-fXc1iZFUA");

            var client = new WebClient();
            var content = client.DownloadString(requestUri);
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(content);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/distance/text");

            var distance = node.InnerXml;
            distance=distance.Replace("mi", "");
            return Convert.ToDouble(distance);
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
                string sql = @"update Scheduler set NumberOfGames=" + Sch.NumberOfGames + ",ScheduleType='" + Sch.ScheduleType + "',Age='" + Sch.AgeDivision + "',StartDate='" + Sch.StartDate.Value.ToString("yyyy/MM/dd") + "',EndDate='" + Sch.EndDate.Value.ToString("yyyy/MM/dd") + "',BracketRule='" + Sch.BracketRules + "',StartTime='" + Sch.FromTime + "',EndTime='" + Sch.EndTime + "',TimeBetweenGames='" + Sch.TimeBetweenGames + "',Monday=" + Sch.Monday + ",Tuesday=" + Sch.Tuesday + ",Wednasday=" + Sch.Wednasday + ",Thursday=" + Sch.Thursday + ",Friday=" + Sch.Friday + ",Saturday=" + Sch.Saturday + ",Sunday=" + Sch.Sunday + ",GameDuration=" + Sch.GameDuration + " where scheduleid = " + Sch.ScheduleId;  
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
                    Scheduler sch = new Scheduler { ScheduleId = (int)rdr["ScheduleId"], AgeDivision = rdr["Age"].ToString(), ScheduleType = rdr["ScheduleType"].ToString(), NumberOfGames = (int)rdr["NumberOfGames"], StartDate = (DateTime)rdr["StartDate"], EndDate = (DateTime)rdr["EndDate"], IsTournamentScheduleCreated=false };
                    Model.Add(sch);
                }
                rdr.Close();
                foreach (var t in Model)
                {
                    sql = "select * from tournamentschedule where scheduleid=" + t.ScheduleId;
                    cmd = new MySqlCommand(sql, mCn);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read()) t.IsTournamentScheduleCreated = true;
                    rdr.Close();
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
                    Scheduler Model = new Scheduler { ScheduleId = (int)rdr["ScheduleId"], ScheduleType = rdr["ScheduleType"].ToString(), NumberOfGames = (int)rdr["NumberOfGames"], StartDate = ((DateTime)rdr["StartDate"]), EndDate = (DateTime)rdr["EndDate"], AgeDivision = (string)rdr["Age"], BracketRules = (string)rdr["BracketRule"], EndTime = (TimeSpan)rdr["EndTime"], FromTime = (TimeSpan)rdr["StartTime"], TimeBetweenGames = (int)rdr["TimeBetweenGames"], Sunday = (bool)rdr["Sunday"], Monday = (bool)rdr["Monday"], Tuesday = (bool)rdr["Tuesday"], Wednasday = (bool)rdr["Wednasday"], Thursday = (bool)rdr["Thursday"], Friday = (bool)rdr["Friday"], Saturday = (bool)rdr["Saturday"], GameDuration = (int)rdr["GameDuration"] };
                    rdr.Close();
                    mCn.Close();
                     return Model;
                }
                return new Scheduler();
            }
        }

        public List<TournamentSchedule> GetTournamentScheduleById(int ScheduleId)
        {
            List<TournamentSchedule> lst=new List<TournamentSchedule>();
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select a.teamname as teamone,b.teamname as teamtwo,l.locationname,t.gamedate,t.starttime from tournamentschedule t inner join locations l on t.locationid=l.locationid inner join teams a on t.teamone=a.teamid inner join teams b on t.teamtwo=b.teamid where scheduleid=" + ScheduleId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                //rdr.Read();
                while (rdr.Read())
                {
                    TournamentSchedule Model = new TournamentSchedule { TeamOne = (string)rdr["teamone"], TeamTwo = (string)rdr["teamtwo"].ToString(), Location = (string)rdr["locationname"], GameDate = Convert.ToDateTime(rdr["gamedate"]).ToShortDateString(), GameTime = (TimeSpan)rdr["starttime"] };
                    lst.Add(Model);
                }
                rdr.Close();
                mCn.Close();
                return lst;
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
        
        List<int> GetTeamsInSchedule(int ScheduleId)
        {
            using (MySqlConnection mCn = new MySqlConnection(connectionString))
            {
                mCn.Open();
                string sql = "select TeamId from tournamentsteams where scheduleid=" + ScheduleId;
                MySqlCommand cmd = new MySqlCommand(sql, mCn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<int> Teams = new List<int>();
                while (rdr.Read())
                {
                    Teams.Add(Convert.ToInt16(rdr["TeamId"]));
                }
                return Teams;
            }
        }
    }
}