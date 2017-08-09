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
    public class GetAdminListController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public ModelGetEmployeeList GetEmployeeList()
        {
            ModelGetEmployeeList obj = new ModelGetEmployeeList();

            List<EmployeeList> empList = new List<EmployeeList>();
            DataTable dt = new DataTable();

            string q = "select uid,fname,lname from nworksuser where _type='Admin' or _type='Admin_Employee';";
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); ;
            conn.Open();
            adapter.Fill(dt);
            conn.Close();

            foreach (DataRow row in dt.Rows)
            {
                EmployeeList employee = new EmployeeList();
                employee.uid = row["uid"].ToString();
                employee.fname = row["fname"].ToString();
                employee.lname = row["lname"].ToString();

                empList.Add(employee);
            }

            obj.ServiceStatus = "Success";
            obj.EmpList = empList;
            return obj;
        }
    }
}
