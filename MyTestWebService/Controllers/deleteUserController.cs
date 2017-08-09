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
    public class deleteUserController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string deleteUser(getUid userId)
        {
            try
            {
                string q = "delete from nworksuser where uid='" + userId.uid + "';";
                MySqlCommand cmd = new MySqlCommand(q, conn);
                MySqlDataReader rdr;
                conn.Open();
                rdr = cmd.ExecuteReader();
                conn.Close();
                return string.Format("User Deleted Successfully!");
            }catch(Exception ex)
            {
                return string.Format(ex.ToString());
            } 
        }
    }
}
