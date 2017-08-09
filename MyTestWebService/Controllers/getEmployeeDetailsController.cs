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
    public class getEmployeeDetailsController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public EmployeeDetails GetEmployeeList(string uid)
        {
            EmployeeDetails obj = new EmployeeDetails();

            List<Model_EmployeeList> emp = new List<Model_EmployeeList>();

            DataTable dt = new DataTable();

            string q = "select * from nworksuser where uid='" + uid + "';";
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); ;
            conn.Open();
            adapter.Fill(dt);
            conn.Close();


            string q1 = "select * from additional_earned_leaves where uid='" + uid + "' and current_year='" + DateTime.Today.Year.ToString() + "';";
            MySqlCommand cmd1 = new MySqlCommand(q1,conn);
            float RolledLeaves = 0;
            float EarnedLeaves = 0;
            conn.Open();
            MySqlDataReader rdr1 = cmd1.ExecuteReader();
            while(rdr1.Read())
            {
                RolledLeaves = rdr1.GetFloat("Rolled_Over_from_Previous_Year");
                EarnedLeaves = rdr1.GetFloat("Earned_in_current_year");
            }
            conn.Close();


            Model_EmployeeList employee = new Model_EmployeeList();


            foreach (DataRow row in dt.Rows)
            {

                employee.uid = row["uid"].ToString();
                employee.fname = row["fname"].ToString();
                employee.mname = row["mname"].ToString();
                employee.lname = row["lname"].ToString();
                employee.dob = Convert.ToDateTime(row["dob"]);
                employee.gender = row["gender"].ToString();
                employee.desg = row["desg"].ToString();
                employee.hire_date = Convert.ToDateTime(row["hire_date"]);
                employee.username = row["username"].ToString();
                employee.addressLine1 = row["addressLine1"].ToString();
                employee.addressLine2 = row["addressLine2"].ToString();
                employee.state = row["state"].ToString();
                employee.city = row["city"].ToString();
                employee.pincode = row["pincode"].ToString();
                employee.country = row["country"].ToString();
                employee.leaveDate = Convert.ToDateTime(row["leaveDate"]);
                employee.anniversary = Convert.ToDateTime(row["annivarsary"]);
                employee.email = row["email"].ToString();
                employee.mobileNo = row["mobileno"].ToString();
                employee.userActive = row["useractive"].ToString();
                employee.Leaves_Rolled_Over_from_Previous_yr = RolledLeaves;
                employee.Additional_Earned_Leaves = EarnedLeaves;
                emp.Add(employee);
            }

            obj.ServiceStatus = "Success";
            obj.Emp = employee;
            return obj;
        }
    }
}
