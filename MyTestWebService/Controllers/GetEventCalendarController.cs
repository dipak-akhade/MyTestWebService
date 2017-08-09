using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System.Data;

namespace MyTestWebService.Controllers
{
    public class GetEventCalendarController : ApiController
    {
        private string q="";
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpGet]
        public List<ModelEventCalendar> GetEventCalendar()
        {
            q = "select srno,occasion,_date,week_day from holiday_calendar order by _date;";
            List<ModelEventCalendar> Calendar = new List<ModelEventCalendar>();
            
            MySqlCommand cmd = new MySqlCommand(q, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int i = 1;
            while(rdr.Read())
            {
                ModelEventCalendar obj = new ModelEventCalendar();
                obj.srno = i.ToString();
                obj.Occasion = rdr.GetString("occasion");
                obj.OccasionDate = rdr.GetDateTime("_date").ToString("dd-MMM-yyyy");
                obj.OccasionWeekDay = rdr.GetString("week_day");
                Calendar.Add(obj);
                i++;
            }

            return Calendar;
        }
    }
}
