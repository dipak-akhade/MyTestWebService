using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyTestWebService.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyTestWebService.Controllers
{
    public class userLoginController : ApiController
    {
        MySqlConnection connection;

        private userLogin userLodinStatus = new userLogin();
        string Query = "";
        string Query1 = "";
        [HttpPost]
        public userLogin UserLogin(login objLogin)
        {
            connection = new MySqlConnection(Constants.conn);
            connection.Open();
            Query = "select count(*) from nWorksUser where Username ='" + objLogin.username + "'and _password='" + encryptDecrypt.Encrypt(objLogin.password.ToString()) + "';";
            MySqlCommand cmd = new MySqlCommand(Query, connection);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0)
            {
                userLodinStatus.serviceStatus = "Failure";
                userLodinStatus.userActive = "null";
                userLodinStatus.userId = "null";
                userLodinStatus.type = "null";
                return userLodinStatus;
                cmd.Dispose();
                connection.Close();
            }
            else
            {
                Query1 = "select * from nWorksUser where Username ='" + objLogin.username + "'and _password='" + encryptDecrypt.Encrypt(objLogin.password.ToString()) + "';";

                MySqlCommand cmd1 = new MySqlCommand(Query1, connection);

                MySqlDataReader reader = cmd1.ExecuteReader();
                string db_pwd = "", userId = "", type = "", userActive = "", user_name = "";
                while (reader.Read())
                {
                    user_name = reader.GetString("Username");
                    db_pwd = reader.GetString("_password");
                    userId = reader.GetString("uid");
                    type = reader.GetString("_type");
                    userActive = reader.GetString("UserActive");
                }
                cmd.Dispose();
                connection.Close();

                userLodinStatus.serviceStatus = "Success";
                userLodinStatus.userActive = userActive;
                userLodinStatus.userId = userId.ToString();
                userLodinStatus.type = type.ToString();
                return userLodinStatus;
            }
        }
    }
}