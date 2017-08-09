using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class ManageEmployeeController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public string userRegistration(userRegistration userUpdate)
        {
            try
            {
                string Query = "update nWorksUser set fname='" + userUpdate.fname + "',mname='" + userUpdate.mname + "',lname='" + userUpdate.lname + "',dob='" + userUpdate.dob + "',gender='" + userUpdate.gender + "',desg='" + userUpdate.desg + "',hire_date='" + userUpdate.hire_date + "',username='" + userUpdate.username + "',addressline1='" + userUpdate.addressLine1 + "',addressline2='" + userUpdate.addressLine2 + "',state='" + userUpdate.state + "',city='" + userUpdate.city + "',pincode='" + userUpdate.pincode + "',country='" + userUpdate.country + "',mobileno='" + userUpdate.mobileNo + "',_type='" + userUpdate._type + "',useractive='" + userUpdate.userActive + "',leavedate='" + userUpdate.leaveDate + "',annivarsary='" + userUpdate.anniversary + "',email='" + userUpdate.email + "' where uid='" + userUpdate.uid + "';";
                MySqlCommand MyCommand = new MySqlCommand(Query, conn);
                MySqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                conn.Close();

                string q = "update additional_earned_leaves set Rolled_Over_from_Previous_Year='" + userUpdate.Leaves_Rolled_Over_from_Previous_yr + "',Earned_in_current_year='" + userUpdate.Additional_Earned_Leaves + "' where uid='" + userUpdate.uid + "' and Current_Year='" + DateTime.Today.Year + "';";
                MySqlCommand cmd = new MySqlCommand(q, conn);
                MySqlDataReader rdr;
                conn.Open();
                rdr = cmd.ExecuteReader();
                conn.Close();


                return string.Format("Upadation Successful!");
            }
            catch (Exception ex)
            {
                return string.Format("Failure in Upadation! Please Contact Admin!");
            }
        }
    }
}
