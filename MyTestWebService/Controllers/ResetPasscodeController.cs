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
    public class ResetPasscodeController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string resetPass(resetPassword obj)
        {
            string response = "";
            try
            {
                string q = "update nworksuser set _password='" + encryptDecrypt.Encrypt(obj.password) + "' where uid='" + obj.uid.ToString() + "';";
                MySqlCommand cmd = new MySqlCommand(q,conn);
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();

                string q0 = "delete from recoverpwdstatus where uid='" + obj.uid.ToString() + "';";
                MySqlCommand cmd0 = new MySqlCommand(q0, conn);
                conn.Open();
                MySqlDataReader rdr0 = cmd0.ExecuteReader();
                conn.Close();

                response = "Password Successfully updated!";



                return response;
            }
            catch (Exception ex)
            {
                return string.Format(ex.ToString());
            }

        }
    }
}
