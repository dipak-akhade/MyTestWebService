using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MyTestWebService.Controllers
{
    public class usersHolidayStatusController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public userHolidaysStatus userLeaveStatus(getUid user)
        {
            float a_earned_in_current_year = 0;
            int Current_Year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(Current_Year, 1, 1);
            DateTime lastDay = new DateTime(Current_Year, 12, 31);
            int Current_Month = DateTime.Now.Month;
            int y2ydiff = 0;
            int y2mdiff = 0;
            float stdYearlyHolidays = 8;
            int Current_Day = DateTime.Now.DayOfYear;


            userHolidaysStatus MyData = new userHolidaysStatus();

            string q2 = "select * from nworksuser where uid='" + user.uid + "';";
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

            if (Date_of_joining.Year.ToString() == Current_Year.ToString())
            {
                Current_Day = Convert.ToInt32((DateTime.Today.Date - Date_of_joining).TotalDays);
            }
            if (Date_of_joining.Year.ToString() == Current_Year.ToString())
            {
                DateTime endOfMonth = new DateTime(Current_Year, Current_Month, DateTime.DaysInMonth(Current_Year, Current_Month));

                y2mdiff = Convert.ToInt32((endOfMonth - Date_of_joining).TotalDays);
                y2ydiff = Convert.ToInt32((lastDay - Date_of_joining).TotalDays);

            }
            else
            {
                DateTime endOfMonth = new DateTime(Current_Year, Current_Month, DateTime.DaysInMonth(Current_Year, Current_Month));

                y2mdiff = Convert.ToInt32((endOfMonth - firstDay).TotalDays);
                y2ydiff = Convert.ToInt32((lastDay - firstDay).TotalDays);

            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Calculate A
            if (IsUserActive == "false")
            {
                double EarnedHolidays;
                if (Date_of_joining.Year == DateTime.Today.Year)
                    EarnedHolidays = (((leavedate - Date_of_joining).TotalDays + 1) / 365) * 8;
                else
                    EarnedHolidays = (((leavedate - firstDay).TotalDays + 1) / 365) * 8;
                MyData.a_earned_in_current_year = float.Parse(Math.Round(EarnedHolidays, 1, MidpointRounding.AwayFromZero).ToString());
                a_earned_in_current_year = float.Parse(EarnedHolidays.ToString());
                //A)
                Debug.WriteLine(EarnedHolidays);
            }
            else
            {
                double EarnedHolidays;
                if (Date_of_joining.Year == DateTime.Today.Year)
                    EarnedHolidays = (((DateTime.Today - Date_of_joining).TotalDays + 1) / 365) * 8;
                else
                    EarnedHolidays = (((DateTime.Today - firstDay).TotalDays + 1) / 365) * 8;
                MyData.a_earned_in_current_year = float.Parse(Math.Round(EarnedHolidays, 1, MidpointRounding.AwayFromZero).ToString());
                a_earned_in_current_year = float.Parse(EarnedHolidays.ToString());
                //A)
                Debug.WriteLine(EarnedHolidays);
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Calculate B
            string q3 = "select count(*) from Leaves where  empno='" + user.uid + "' and holiday='Holiday' and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + DateTime.Today.ToString("yyyy-MM-dd") + "';";
            conn.Open();
            MySqlCommand cmd3 = new MySqlCommand(q3, conn);
            float usedHolidaysCount = float.Parse(Convert.ToInt32(cmd3.ExecuteScalar()).ToString());
            conn.Close();
            MyData.b_total_used_in_current_year = usedHolidaysCount;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Calculate C
            string q5 = "select count(*) from Leaves where  empno='" + user.uid + "' and holiday='Holiday' and leavedate between '" + DateTime.Today.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "';";
            conn.Open();
            MySqlCommand cmd5 = new MySqlCommand(q5, conn);
            float not_usedHolidaysCount = float.Parse(Convert.ToInt32(cmd5.ExecuteScalar()).ToString());
            conn.Close();
            MyData.c_total_applied_but_notused_in_current_year = not_usedHolidaysCount;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //calculate E
            float All_Taken_Holidays = MyData.b_total_used_in_current_year + MyData.c_total_applied_but_notused_in_current_year;

            float e_total_left_in_current_year2date = stdYearlyHolidays / 365 * Current_Day - All_Taken_Holidays;

            string[] MyString0 = e_total_left_in_current_year2date.ToString().Split('.');
            if (MyString0.Length == 2)
            {

                e_total_left_in_current_year2date = float.Parse(MyString0[0]);
               
            }
            MyData.e_total_left_in_current_year2date = e_total_left_in_current_year2date;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculate D

            float d_total_left_in_current_year = (stdYearlyHolidays / 365*y2ydiff) - (MyData.b_total_used_in_current_year + MyData.c_total_applied_but_notused_in_current_year);
            string[] MyString = d_total_left_in_current_year.ToString().Split('.');
            if (MyString.Length == 2)
            {
                d_total_left_in_current_year = float.Parse(MyString[0]);
            } 
            MyData.d_total_left_in_current_year = d_total_left_in_current_year;
            //D)

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculate F
            float f_total_left_in_current_year2Month = (stdYearlyHolidays / 365 * y2mdiff) - All_Taken_Holidays;
            string[] MyString1 = f_total_left_in_current_year2Month.ToString().Split('.');
            if (MyString1.Length == 2)
            {
                f_total_left_in_current_year2Month = float.Parse(MyString1[0]);
            }
            MyData.f_total_left_in_current_year2Month = f_total_left_in_current_year2Month;

            return MyData;
        }
    }
}
