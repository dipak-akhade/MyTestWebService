using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MyTestWebService.Controllers
{
    public class GetEmployeeListController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public List<OnlyEmployee> GetEmployeeList()
        {
            List<OnlyEmployee> obj = new List<OnlyEmployee>();
            DataTable dt = new DataTable();
            
            string q = "select uid,fname,lname from nworksuser where _type='Employee' or _type='Admin_Employee' ;";
            MySqlCommand cmd = new MySqlCommand(q,conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); ;
            conn.Open();
            adapter.Fill(dt);

            DataView dataview = dt.DefaultView;
            dataview.Sort = "fname asc";
            DataTable dtt = dataview.ToTable();
            conn.Close();

            foreach (DataRow row in dtt.Rows)
            {
                OnlyEmployee employee = new OnlyEmployee();
                employee.id = row["uid"].ToString();
                employee.empName = row["fname"].ToString()+" "+ row["lname"].ToString();
                employee.check = false;
                obj.Add(employee);
            }           
            return obj;
        }
    }
}
