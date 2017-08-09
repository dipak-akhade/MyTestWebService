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
    public class DeleteEventsController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string deleteEvent(delete_Event obj)
        {
            try
            {
                string q0 = "select srno from holiday_calendar where occasion='" + obj.event2delete + "';";
                MySqlCommand cmd0 = new MySqlCommand(q0, conn);
                conn.Open();
                MySqlDataReader rdr0 = cmd0.ExecuteReader();
                string srno = "";
                while (rdr0.Read())
                {
                    srno = rdr0.GetString("srno");
                }
                conn.Close();

                string q = "delete from holiday_calendar where SrNo='" + srno + "'";
                MySqlCommand cmd = new MySqlCommand(q, conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
                return string.Format(obj.event2delete.ToString()+" is deleted successfully!");
            }
            catch(Exception ex)
            {
                return string.Format(ex.ToString());
            }
            

        }
    }
}
