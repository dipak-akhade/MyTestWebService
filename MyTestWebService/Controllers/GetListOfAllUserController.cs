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
    public class GetListOfAllUserController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpGet]
        public List<modelUserID_Username> GetListOfAllUser()
        {
            List<modelUserID_Username> mainData = new List<modelUserID_Username>();

            DataTable dt = new DataTable();

            string q = "select uid,Username from nworksuser;";
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); ;
            conn.Open();
            adapter.Fill(dt);
            DataView dataview = dt.DefaultView;
            dataview.Sort = "Username asc";
            DataTable dtt = dataview.ToTable();

            conn.Close();

            foreach (DataRow row in dtt.Rows)
            {
                modelUserID_Username data = new modelUserID_Username();

                data.uid = row["uid"].ToString();
                data.username = row["username"].ToString();

                mainData.Add(data);
            }

            return mainData;
        }

    }
}
