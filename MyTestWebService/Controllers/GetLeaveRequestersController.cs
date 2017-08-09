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
    public class GetLeaveRequestersController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        string q = "select distinct(n.uid) userid,fname,lname from nworksuser n, temporaryrequesteddates t where t.uid=n.uid and  n.uid in (select distinct uid from temporaryrequesteddates);";
        //string q = "select distinct(n.uid) userid,fname,lname,t.requestedon Requeston from nworksuser n, temporaryrequesteddates t where t.uid=n.uid and  n.uid in (select distinct uid from temporaryrequesteddates);";
        [HttpGet]
        public List<LeaveRequester> GetLeaveRequesters()
        {
            List<LeaveRequester> Requesters = new List<LeaveRequester>();

            conn.Open();
            DataTable table = new DataTable();
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(table);
            conn.Close();
            foreach (DataRow row in table.Rows)
            {
                LeaveRequester Requester = new LeaveRequester();
                Requester.userId = row["userid"].ToString();
                Requester.userFullName = row["fname"].ToString()+" "+ row["lname"].ToString();
                Requester.requestedOn = "";
                //Requester.requestedOn =Convert.ToDateTime(row["Requeston"].ToString()).ToString("dd/MMM/yyyy");
                Requesters.Add(Requester);
            }
            return Requesters;
        }
    }
}
