using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System.Data;
using System.Diagnostics;

namespace MyTestWebService.Controllers
{
    public class getEventCalendarForUserController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public List<ModelEventCalendarForUser> GetEventCalendar(getUid user)
        {
            string q1 = "select srno,occasion,_date,week_day from holiday_calendar order by _date;";
            string q2 = "select count(*) from leaves where empno='" + user.uid + "';";
            string q3 = "select leavedate from leaves where empno='" + user.uid + "';";

            List<ModelEventCalendarForUser> Calendar = new List<ModelEventCalendarForUser>();

            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
            Int32 empLeaveCount = Convert.ToInt32(cmd2.ExecuteScalar());
            conn.Close();

            conn.Open();
            DataTable table = new DataTable();
            MySqlCommand cmd1 = new MySqlCommand(q1, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd1);
            adapter.Fill(table);
            conn.Close();

            foreach (DataRow row in table.Rows)
            {
                ModelEventCalendarForUser obj = new ModelEventCalendarForUser();

                obj.Occasion = row["occasion"].ToString();
                obj.OccasionDate = Convert.ToDateTime(row["_date"].ToString()).ToString("dd-MMM-yyyy");
                obj.OccasionWeekDay = row["week_day"].ToString();

                //check whether user already taken current ocassion
                if (empLeaveCount == 0)
                {
                    Debug.WriteLine("Marking all as true.");
                    obj.isAllow = true;
                    Calendar.Add(obj);
                    continue;
                }

                conn.Open();
                MySqlCommand cmd3 = new MySqlCommand(q3, conn);
                MySqlDataReader rdr1 = cmd3.ExecuteReader();
                while (rdr1.Read())
                {
                    string leaveTaken = rdr1.GetString("leavedate");
                    Debug.WriteLine("Leave Taken: "+leaveTaken);
                    string eventDate = row["_date"].ToString();
                    Debug.WriteLine("Event Date: " + eventDate);
                    if (leaveTaken.Equals(eventDate))
                    {
                        Debug.WriteLine("Inside If");
                        obj.isAllow = false;                        
                        break;
                    }
                    Debug.WriteLine("Marking "+leaveTaken+" as true.");
                    obj.isAllow = true;
                }
                Debug.WriteLine("");
                Calendar.Add(obj);
                rdr1.Close();
                conn.Close();
            }
            return Calendar;
        }
    }
}
