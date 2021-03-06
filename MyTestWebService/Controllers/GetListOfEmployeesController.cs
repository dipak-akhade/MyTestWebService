﻿using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class GetListOfEmployeesController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public ModelGetEmployeeList GetEmployeeList()
        {

            ModelGetEmployeeList obj = new ModelGetEmployeeList();
            DataTable dt = new DataTable();

            string q = "select uid,fname,lname from nworksuser where _type='Employee'  or _type='Admin_Employee';";
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); ;
            conn.Open();
            //dt.DefaultView.Sort = "fname";
            adapter.Fill(dt);

            DataView dataview = dt.DefaultView;
            dataview.Sort = "fname asc";
            DataTable dtt = dataview.ToTable();
            
            conn.Close();       


            List<EmployeeList>Employees = new List<EmployeeList>();

            foreach (DataRow row in dtt.Rows)
            {
                EmployeeList employee = new EmployeeList();
                employee.uid = row["uid"].ToString();
                employee.fname = row["fname"].ToString();
                employee.lname = row["lname"].ToString();
                Employees.Add(employee);
            }
            obj.ServiceStatus = "Success";
            obj.EmpList = Employees;


            return obj;
        }
    }
}
