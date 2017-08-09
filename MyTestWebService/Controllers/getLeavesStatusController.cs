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
    public class getLeavesStatusController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        int totalLeaves = 22,totalHolidays=8;
        [HttpGet]
        public LeavesStatus getLeavesStatus(string uid)
        {
            //getting balance leaves
            LeavesStatus obj = new LeavesStatus();
            string q0="",q1 = "", q2 = "", q3 = "",q4="",q5="";

            int Current_Year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(Current_Year, 1, 1);
            DateTime lastDay = new DateTime(Current_Year, 12, 31);
            


            q1 = "select count(*) from leaves where holiday='leave' and EmpNo='" + uid + "' and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "';";
            MySqlCommand cmd1 = new MySqlCommand(q1, conn);
            conn.Open();
            Int32 leavesTaken = Convert.ToInt32(cmd1.ExecuteScalar());//leaves taken
            conn.Close();

            q0 = "select count(*) from leaves where holiday='Half Day' and EmpNo='" + uid + "'  and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "';";
            MySqlCommand cmd0 = new MySqlCommand(q0, conn);
            conn.Open();
            double HalfDay_leavesTaken = Convert.ToDouble(cmd0.ExecuteScalar());
            double HalfDayLeaveCount = (HalfDay_leavesTaken )/ 2;   //half leaves taken
            conn.Close();

            double balanceLeaves = totalLeaves - leavesTaken-HalfDayLeaveCount;
            obj.balanceLeaves = balanceLeaves;

            //getting balance holidays
            q2 = "select count(*) from leaves where holiday='holiday' and EmpNo='" + uid + "' and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "';";
            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
            conn.Open();
            Int32 holidaysTaken = Convert.ToInt32(cmd2.ExecuteScalar());
            conn.Close();
            int balanceHolidays = totalHolidays - holidaysTaken;
            obj.balanceHolidays = balanceHolidays;

            //list of holidays taken
            q3 = "select * from leaves where holiday='Holiday' and EmpNo='" + uid + "'  order by leavedate asc;";
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
            obj.holidays = userHolidays ;

            //list of leaves taken
            q4 = "select * from leaves where (holiday='Leave' or holiday='Half Day') and EmpNo='" + uid + "'  order by leavedate asc;";
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

            obj.totalHolidays = totalHolidays;

            obj.totalLeaves = totalLeaves;

            return obj;
        }
    }
}
