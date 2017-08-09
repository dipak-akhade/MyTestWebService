using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;

namespace MyTestWebService.Controllers
{
    public class usersLeavesStatusController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public userLeavesStatus userLeaveStatus([FromBody]string uid)
        {
            userLeavesStatus MyData = new userLeavesStatus();

            string q1 = "select * from additional_earned_leaves where uid='" + uid + "' ;";
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(q1, conn);
            MySqlDataReader rdr1 = cmd1.ExecuteReader();
            while (rdr1.Read())
            {
                //MyData.a_rolled_over_from_previous_year = rdr1.GetFloat("Rolled_Over_from_Previous_Year");
                //MyData.c_additional_earned_in_current_year = rdr1.GetFloat("Earned_in_current_year");
                MyData.a_rolled_over_from_previous_year = 10;
                MyData.c_additional_earned_in_current_year = 20;

            }
            conn.Close();

            MyData.a_rolled_over_from_previous_year = 0;//from admin
            MyData.b_earned_in_current_year = 0;//formula
            MyData.c_additional_earned_in_current_year = 0;//from admin
            MyData.d_total_in_current_year = (MyData.a_rolled_over_from_previous_year + MyData.b_earned_in_current_year + MyData.c_additional_earned_in_current_year);
            MyData.e_total_used_in_current_year = 0;
            MyData.f_total_applied_but_notused_in_current_year = 0;
            MyData.g_total_without_pay_in_current_year = 0;
            MyData.h_total_left_in_current_year = 0;
            return MyData;
        }

    }
}
