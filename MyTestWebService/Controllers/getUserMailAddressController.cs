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
    public class getUserMailAddressController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public string getEmailAddress(getUid user)
        {
            try
            {
                string EmailAddress = "";
                string q = "select email from nworksuser where uid='" + user.uid + "';";
                MySqlCommand cmd = new MySqlCommand(q, conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmailAddress = rdr.GetString("email");
                }
                conn.Close();

                return EmailAddress;
            }
            catch(Exception ex)
            {
                return string.Format(ex.ToString());
            }
            
        }

    }
}
