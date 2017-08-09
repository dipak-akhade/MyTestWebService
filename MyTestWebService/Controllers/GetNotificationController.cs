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
    public class GetNotificationController : ApiController
    {
        [HttpGet]
        public int getNotification()
        {
            Int32 noti = 0;

            MySqlConnection conn = new MySqlConnection(Constants.conn);

            string q = "select count(distinct uid) notifications from temporaryrequesteddates ";

            MySqlCommand cmd = new MySqlCommand(q,conn);

            conn.Open();

            MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                noti = rdr.GetInt32("notifications");
            }

            return noti;
        }
    }
}
