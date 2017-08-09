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

    public class EmployeeToAdminRegistrationController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public string EmployeeToAdminReg(userUpdation update)
        {
            try
            {
                string q1 = "", q2 = "";
                q1 = "select count(*) from nworksuser where uid='" + update.uid + "';";
                MySqlCommand cmd1 = new MySqlCommand(q1, conn);
                conn.Open();
                Int32 count = Convert.ToInt32(cmd1.ExecuteScalar());
                conn.Close();
                cmd1.Dispose();
                if (count != 0)
                {
                    q2 = "update nWorksUser set fname='" + update.fname + "',mname='" + update.mname + "',lname='" + update.lname + "',dob='" + update.dob + "',gender='" + update.gender + "',desg='" + update.desg + "',hire_date='" + update.hire_date + "',username='" + update.username + "',addressline1='" + update.addressLine1 + "',addressline2='" + update.addressLine2 + "',state='" + update.state + "',city='" + update.city + "',pincode='" + update.pincode + "',country='" + update.country + "',mobileno='" + update.mobileNo + "',_type='" + update._type + "',useractive='" + update.userActive + "',leavedate='" + update.leaveDate + "',annivarsary='" + update.anniversary + "',email='" + update.email + "' where uid='" + update.uid + "';";
                    MySqlCommand MyCommand = new MySqlCommand(q2, conn);
                    MySqlDataReader MyReader;
                    conn.Open();
                    MyReader = MyCommand.ExecuteReader();
                    conn.Close();
                    return string.Format("Registration is Successful!");
                }
                else
                {
                    return string.Format(update.fname + " " + update.lname + " is not found!");
                }
            }
            catch(Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
    }
}
