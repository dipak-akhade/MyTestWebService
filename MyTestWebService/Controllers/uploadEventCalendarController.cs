using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using MyTestWebService.Models;

namespace MyTestWebService.Controllers
{
    public class uploadEventCalendarController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        MySqlConnection conn2 = new MySqlConnection(Constants.conn);
        string query1 = "", query2 = "", query3 = "";
        int inserted , updated ;

        [HttpPost]
        public string uploadEventCalendar(List<ModelEventCalendar> events)
        {
            try
            {
                for (int i = 0; i < events.Count; i++)
                {
                    //inserted = 0;
                    //updated = 0;

                    //query1 = "select * from holiday_calendar;";
                    //MySqlCommand command = new MySqlCommand(query1, conn);
                    //MySqlDataReader reader;
                    //conn.Open();
                    //reader = command.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    if (reader.GetString("occasion") == events[i].Occasion.ToString())
                    //    {
                    //        query3 = "update holiday_calendar set _date='" + Convert.ToDateTime(events[i].OccasionDate).ToString("yyyy-MM-dd HH:mm:ss") + "',week_day='" + events[i].OccasionWeekDay.ToString() + "' where occasion='" + events[i].Occasion.ToString() + "';";
                    //        MySqlCommand cmd3 = new MySqlCommand(query3, conn2);
                    //        conn2.Open();
                    //        MySqlDataReader rdr3 = cmd3.ExecuteReader();
                    //        conn2.Close();
                    //        updated++;
                    //    }
                    //    else
                    //    {
                            query2 = "Insert into holiday_calendar(occasion,_date,week_day) values('" + events[i].Occasion + "','" + events[i].OccasionDate + "','" + events[i].OccasionWeekDay + "');";
                            MySqlCommand cmd = new MySqlCommand(query2, conn2);
                            MySqlDataReader rdr;
                            conn2.Open();
                            rdr = cmd.ExecuteReader();
                            conn2.Close();
                            //inserted++;
                //        }
                //    }
                //    conn.Close();                   
                }

                //return string.Format("Calendar Upload is Successful!\n"+inserted.ToString()+" records inserted.\n"+updated.ToString()+" records updated.");
                return string.Format("Calendar Upload is Successful!");
            }
            catch(Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
    }
}

