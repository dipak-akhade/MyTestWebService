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
    public class deleteAdminController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string deleteUser(getUid userId)
        {
            try
            {
                string q1 = "select _type from nworksuser where uid='" + userId.uid + "';";
                MySqlCommand command = new MySqlCommand(q1, conn);
                conn.Open();
                MySqlDataReader rdr1 = command.ExecuteReader();
                string _type = "";
                while (rdr1.Read())
                {
                    _type = rdr1.GetString("_type");
                }
                conn.Close();
                if (_type == "Admin")
                {
                    string q = "delete from nworksuser where uid='" + userId.uid + "';";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    MySqlDataReader rdr;
                    conn.Open();
                    rdr = cmd.ExecuteReader();
                    conn.Close();
                }
                else if (_type == "Admin_Employee")
                {
                    string q = "update nworksuser set _type='Employee'  where uid='" + userId.uid + "';";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    MySqlDataReader rdr;
                    conn.Open();
                    rdr = cmd.ExecuteReader();
                    conn.Close();
                }
                return string.Format("User Deleted Successfully!");
            }
            catch(Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
    }
}
