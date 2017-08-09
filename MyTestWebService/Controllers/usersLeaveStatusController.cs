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
    public class usersLeaveStatusController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        

        [HttpPost]
        public userLeavesStatus userLeaveStatus(getUid user)
        {
            int Current_Year = DateTime.Now.Year;
            int Current_Month = DateTime.Now.Month;
            int Current_Day = DateTime.Now.DayOfYear;
            DateTime firstDay = new DateTime(Current_Year, 1, 1);
            DateTime lastDay = new DateTime(Current_Year, 12, 31);
            int y2ydiff = 0;
            int y2mdiff = 0;
            float stdYearlyLeaves = 22;
            
            userLeavesStatus MyData = new userLeavesStatus();


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
            Debug.WriteLine("Total days : "+(lastDay-firstDay).TotalDays.ToString());

            if (Date_of_joining.Year.ToString() == Current_Year.ToString())
            {
                Current_Day = Convert.ToInt32((DateTime.Today.Date - Date_of_joining).TotalDays);
            }
            if (Date_of_joining.Year.ToString() == Current_Year.ToString() )
            {
                DateTime endOfMonth = new DateTime(Current_Year, Current_Month, DateTime.DaysInMonth(Current_Year, Current_Month));
                                   
                y2mdiff = Convert.ToInt32((endOfMonth - Date_of_joining).TotalDays);
                y2ydiff = Convert.ToInt32((lastDay-Date_of_joining).TotalDays);

            }
            else
            {
                DateTime endOfMonth = new DateTime(Current_Year, Current_Month, DateTime.DaysInMonth(Current_Year, Current_Month));

                y2mdiff = Convert.ToInt32((endOfMonth - firstDay).TotalDays);
                y2ydiff = Convert.ToInt32((lastDay - firstDay).TotalDays);

            }

            //total used in this year
            string q3 = "select count(*) from Leaves where  empno='" + user.uid + "' and holiday='Leave' and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + DateTime.Today.ToString("yyyy-MM-dd") + "';";
            conn.Open();
            MySqlCommand cmd3 = new MySqlCommand(q3, conn);
            float usedLeavesCount = float.Parse(Convert.ToInt32(cmd3.ExecuteScalar()).ToString());
            Debug.WriteLine("Today :" + DateTime.Today.ToShortDateString());
            Debug.WriteLine("first day :" + firstDay.ToShortDateString());
            conn.Close();
            string q4 = "select count(*) from Leaves where  empno='" + user.uid + "' and holiday='Half Day' and leavedate between '" + firstDay.ToString("yyyy-MM-dd") + "' and '" + DateTime.Today.ToString("yyyy-MM-dd") + "';";
            conn.Open();
            MySqlCommand cmd4 = new MySqlCommand(q4, conn);
            float used_HalfDay_Count = float.Parse((Convert.ToDouble(cmd4.ExecuteScalar()) / 2).ToString());
            Debug.WriteLine("Half day count :" + used_HalfDay_Count.ToString());
            conn.Close();
            //applied but not used
            string q5 = "select count(*) from Leaves where  empno='" + user.uid + "' and holiday='Leave' and leavedate between '" + DateTime.Today.ToString("yyyy-MM-dd") + "' and '" + lastDay.ToString("yyyy-MM-dd") + "';";
            conn.Open();
            MySqlCommand cmd5 = new MySqlCommand(q5, conn);
            float not_usedLeavesCount = float.Parse(Convert.ToInt32(cmd5.ExecuteScalar()).ToString());
            conn.Close();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //CALCULATE a AND c

            string q1 = "select * from additional_earned_leaves where Current_Year='" + Current_Year + "' and  uid='" + user.uid + "';";
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(q1, conn);
            MySqlDataReader rdr1 = cmd1.ExecuteReader();
            while (rdr1.Read())
            {
                MyData.a_rolled_over_from_previous_year = rdr1.GetFloat("Rolled_Over_from_Previous_Year");
                MyData.c_additional_earned_in_current_year = rdr1.GetFloat("Earned_in_current_year");
            }
            conn.Close();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculation of B
            double LeavesEarnedUptoYearEnd = 0;
            if (IsUserActive == "false")
            {
                double EarnedLeaves;
                if (Date_of_joining.Year == Current_Year)
                {
                    EarnedLeaves = (((leavedate - Date_of_joining).TotalDays + 1) / 365) * 22;
                    LeavesEarnedUptoYearEnd = EarnedLeaves;

                }
                else
                {
                    EarnedLeaves = (((leavedate - firstDay).TotalDays + 1) / 365) * 22;
                    LeavesEarnedUptoYearEnd = EarnedLeaves;

                }
                MyData.b_earned_in_current_year = float.Parse(Math.Round(EarnedLeaves, 1, MidpointRounding.AwayFromZero).ToString());
                //B)
            }
            else
            {
                double EarnedLeaves;
                if (Date_of_joining.Year == Current_Year)
                {
                    EarnedLeaves = (((DateTime.Today - Date_of_joining).TotalDays + 1) / 365) * 22;
                    LeavesEarnedUptoYearEnd = (((lastDay - Date_of_joining).TotalDays + 1) / 365) * 22;

                }
                else
                {
                    EarnedLeaves = (((DateTime.Today - firstDay).TotalDays + 1) / 365) * 22;
                    LeavesEarnedUptoYearEnd = (((lastDay - firstDay).TotalDays + 1) / 365) * 22;
                }
                MyData.b_earned_in_current_year = float.Parse(Math.Round(EarnedLeaves, 1, MidpointRounding.AwayFromZero).ToString());
                //B)
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Calculations of D
            MyData.d_total_in_current_year =float.Parse( Math.Round((MyData.a_rolled_over_from_previous_year + MyData.b_earned_in_current_year + MyData.c_additional_earned_in_current_year), 1, MidpointRounding.AwayFromZero).ToString());

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Calculations of E            
            MyData.e_total_used_in_current_year = usedLeavesCount + used_HalfDay_Count;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculation of F
           
            MyData.f_total_applied_but_notused_in_current_year = not_usedLeavesCount;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculation of G

            string q11 = "select count(*) from leaves where iswithpay='false' and  empno='" + user.uid + "';";
            conn.Open();
            MySqlCommand cmd11 = new MySqlCommand(q11, conn);
            MyData.g_total_without_pay_in_current_year = float.Parse(Convert.ToInt32(cmd11.ExecuteScalar()).ToString());
            conn.Close();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculation of I

            float i_total_left_in_current_year2date = (MyData.a_rolled_over_from_previous_year + MyData.c_additional_earned_in_current_year + (stdYearlyLeaves / 365) * Current_Day ) - MyData.e_total_used_in_current_year - MyData.f_total_applied_but_notused_in_current_year + MyData.g_total_without_pay_in_current_year;
            string[] MyString0 = i_total_left_in_current_year2date.ToString().Split('.');
            if (MyString0.Length == 2)
            {
                if (Convert.ToDouble("0." + MyString0[1]) < 0.3)
                {
                    MyString0[1] = "0.0";
                    i_total_left_in_current_year2date = float.Parse(MyString0[0]) + float.Parse(MyString0[1]);

                }
                else if (Convert.ToDouble("0." + MyString0[1]) > 0.8)
                {
                    MyString0[1] = "1.0";
                    i_total_left_in_current_year2date = float.Parse(MyString0[0]) + float.Parse(MyString0[1]);
                }
                else
                {
                    MyString0[1] = "0.5";
                    i_total_left_in_current_year2date = float.Parse(MyString0[0]) + float.Parse(MyString0[1]);
                }
            }
            MyData.i_total_left_in_current_year2date = i_total_left_in_current_year2date;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculations of H

            float h_total_left_in_current_year = MyData.a_rolled_over_from_previous_year + MyData.c_additional_earned_in_current_year + (stdYearlyLeaves / 365 * y2ydiff) - MyData.e_total_used_in_current_year - MyData.f_total_applied_but_notused_in_current_year + MyData.g_total_without_pay_in_current_year;
            //float h_total_left_in_current_year = MyData.a_rolled_over_from_previous_year + MyData.c_additional_earned_in_current_year + float.Parse(LeavesEarnedUptoYearEnd.ToString()) - MyData.e_total_used_in_current_year - MyData.f_total_applied_but_notused_in_current_year + MyData.g_total_without_pay_in_current_year;

            string[] MyString = h_total_left_in_current_year.ToString().Split('.');
            if (MyString.Length == 2)
            {
                if (Convert.ToDouble("0." + MyString[1]) < 0.3)
                {
                    MyString[1] = "0.0";
                    h_total_left_in_current_year = float.Parse(MyString[0]) + float.Parse(MyString[1]);
                    Debug.WriteLine(h_total_left_in_current_year.ToString());

                }
                else if (Convert.ToDouble("0." + MyString[1]) > 0.8)
                {
                    MyString[1] = "1.0";
                    h_total_left_in_current_year = float.Parse(MyString[0]) + float.Parse(MyString[1]);
                    Debug.WriteLine(h_total_left_in_current_year.ToString());
                }
                else
                {
                    MyString[1] = "0.5";
                    h_total_left_in_current_year = float.Parse(MyString[0]) + float.Parse(MyString[1]);
                    Debug.WriteLine(h_total_left_in_current_year.ToString());
                }
            }
            MyData.h_total_left_in_current_year = h_total_left_in_current_year;
            //(H)
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Calculations of J

            float j_total_left_in_current_year2month = (MyData.a_rolled_over_from_previous_year + MyData.c_additional_earned_in_current_year + (stdYearlyLeaves / 365) * y2mdiff) - MyData.e_total_used_in_current_year - MyData.f_total_applied_but_notused_in_current_year + MyData.g_total_without_pay_in_current_year;
            string[] MyString2 = j_total_left_in_current_year2month.ToString().Split('.');
            if (MyString2.Length == 2)
            {
                if (Convert.ToDouble("0." + MyString2[1]) < 0.3)
                {
                    MyString2[1] = "0.0";
                    j_total_left_in_current_year2month = float.Parse(MyString2[0]) + float.Parse(MyString2[1]);

                }
                else if (Convert.ToDouble("0." + MyString2[1]) > 0.8)
                {
                    MyString2[1] = "1.0";
                    j_total_left_in_current_year2month = float.Parse(MyString2[0]) + float.Parse(MyString2[1]);
                    Debug.WriteLine(i_total_left_in_current_year2date.ToString());
                }
                else
                {
                    MyString2[1] = "0.5";
                    j_total_left_in_current_year2month = float.Parse(MyString2[0]) + float.Parse(MyString2[1]);
                }
            }
            MyData.j_total_left_in_current_year2month = j_total_left_in_current_year2month;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return MyData;
        }
    }
}
