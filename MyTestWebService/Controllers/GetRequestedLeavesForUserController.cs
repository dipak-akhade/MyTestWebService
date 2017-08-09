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
    public class GetRequestedLeavesForUserController : ApiController
    {
        [HttpGet]
        public List<RequestedLeavesForUser> GetRequestedLeavesForUser(string user)
        {
            List<RequestedLeavesForUser> Object = new List<RequestedLeavesForUser>();
            MySqlConnection conn = new MySqlConnection(Constants.conn);
            string q = "select * from temporaryrequesteddates where uid='" + user + "';";
            MySqlCommand cmd = new MySqlCommand(q, conn);
            conn.Open();
            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                RequestedLeavesForUser obj = new RequestedLeavesForUser();
                //obj.leavedate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd HH:mm:ss}", rdr.GetString("dates")));
                obj.leavedate = rdr.GetDateTime("dates").ToString("yyyy-MM-dd");
                obj.requestedAs = rdr.GetString("requestedas");
                obj.isAccepted = true;
                obj.isWithPay = true;
                Object.Add(obj);
            }
            conn.Close();
            return Object;
        }
    } 
}
