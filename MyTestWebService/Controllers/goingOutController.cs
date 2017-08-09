using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System.Diagnostics;

namespace MyTestWebService.Controllers
{
    public class goingOutController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        int IsDeviceChanged=1;
        [HttpPost]
        public string GoingOut(comingInANDgoingOut outside)
        {
            try
            {
                string q = "select DeviceID from in_out where uid='"+outside.empId+"' order by inout_id desc limit 1";
                string LastUsedDeviceID = "";
                conn.Open();
                MySqlCommand mycommand = new MySqlCommand(q,conn);
                MySqlDataReader rdr2 = mycommand.ExecuteReader();
                while(rdr2.Read())
                {
                    LastUsedDeviceID = rdr2.GetString("DeviceID");
                }
                conn.Close();
                if(LastUsedDeviceID==outside.DeviceID)
                {
                    IsDeviceChanged = 0;
                }



                string query = "select inout_status from nworksuser where uid='" + outside.empId + "';";
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                string status = command.ExecuteScalar().ToString();
                conn.Close();
                if (status == "Outside")
                {
                    return string.Format("You are already out!");
                }
                else
                {
                    string Query = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviceID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + outside.empId + "','" + "00:00:00" + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + outside.qrValue + "','" + outside.latitude + "','" + outside.longitude + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + outside.DeviceID.ToString() + "','" + outside.Location.ToString() + "','"+outside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                    MySqlCommand cmd = new MySqlCommand(Query, conn);
                    MySqlDataReader rdr;
                    conn.Open();
                    rdr = cmd.ExecuteReader();
                    conn.Close();

                    string Query1 = "update nworksuser set inout_status='Outside' where uid='" + outside.empId + "';";
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(Query1, conn);
                    MySqlDataReader rdr1;
                    rdr1 = cmd1.ExecuteReader();
                    conn.Close();

                    return string.Format("You went OUT successfully!");
                }
            }
            catch (Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
    }
}
