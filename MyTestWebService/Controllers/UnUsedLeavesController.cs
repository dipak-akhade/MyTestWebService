using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class UnUsedLeavesController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public AppliedButNotUsed getLeavesStatus(string uid)
        {
            int Current_Year = DateTime.Now.Year;
            int Current_Month = DateTime.Now.Month;
            DateTime firstDay = new DateTime(Current_Year, 1, 1);
            DateTime lastDay = new DateTime(Current_Year, 12, 31); 

            AppliedButNotUsed obj = new AppliedButNotUsed();

            string q2 = "select * from nworksuser where uid='" + uid + "';";
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
            MySqlDataReader rdr2 = cmd2.ExecuteReader();
            DateTime Date_of_joining = new DateTime();
            string IsUserActive = "";
            DateTime leavedate = DateTime.Today;
            while (rdr2.Read())
            {
                Date_of_joining = rdr2.GetDateTime("hire_date");
                IsUserActive = rdr2.GetString("UserActive");
                leavedate = rdr2.GetDateTime("leavedate");
            }
            conn.Close();

            //list of holidays taken
            string q3 = "select * from Leaves where  empno='" + uid + "' and holiday='Holiday' and leavedate between '" + DateTime.Today.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "' order by leavedate asc;";

            MySqlCommand cmd3 = new MySqlCommand(q3, conn);
            DataTable table1 = new DataTable();
            conn.Open();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter(cmd3);
            adapter1.Fill(table1);
            conn.Close();
            List<Holidays> userHolidays = new List<Holidays>();
            foreach (DataRow row in table1.Rows)
            {
                Holidays l = new Holidays();
                l.date = Convert.ToDateTime(row["leavedate"].ToString()).ToString("dd-MMM-yyyy");
                l.weekday = Convert.ToDateTime(l.date).DayOfWeek.ToString();

                userHolidays.Add(l);
            }
            obj.holidays = userHolidays;

            //list of leaves taken
            string q4 = "select * from Leaves where (holiday='Leave' or holiday='Half Day') and empno='" + uid + "'  and leavedate between '" + DateTime.Today.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "' order by leavedate asc;";

            MySqlCommand cmd4 = new MySqlCommand(q4, conn);
            DataTable table2 = new DataTable();
            conn.Open();
            MySqlDataAdapter adapter2 = new MySqlDataAdapter(cmd4);
            adapter2.Fill(table2);
            conn.Close();
            List<Leaves> userLeaves = new List<Leaves>();
            foreach (DataRow row in table2.Rows)
            {
                Leaves l = new Leaves();
                l.date = Convert.ToDateTime(row["leavedate"].ToString()).ToString("dd-MMM-yyyy"); ;
                l.weekday = Convert.ToDateTime(l.date).DayOfWeek.ToString();
                l.leaveType = row["holiday"].ToString();
                userLeaves.Add(l);
            }
            obj.leaves = userLeaves;

            return obj;
        }
    }
}
