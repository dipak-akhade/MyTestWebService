using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MyTestWebService.Controllers
{
    public class comingInController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        int IsDeviceChanged = 1;

        [HttpPost]
        public string ComingIn(comingInANDgoingOut inside)
        {
            try
            {

                string MyQuery = "select DeviceID from in_out where uid='" + inside.empId + "' order by inout_id desc limit 1";
                string LastUsedDeviceID = "";
                conn.Open();
                MySqlCommand mycommand = new MySqlCommand(MyQuery, conn);
                MySqlDataReader Myrdr = mycommand.ExecuteReader();
                while (Myrdr.Read())
                {
                    LastUsedDeviceID = Myrdr.GetString("DeviceID");
                }
                conn.Close();
                if (LastUsedDeviceID == inside.DeviceID)
                {
                    IsDeviceChanged = 0;
                }

                //get last In date
                string q4 = "select count(*) from in_out where uid='" + inside.empId.ToString() + "' limit 1";//Check if new user or not
                MySqlCommand cmd4 = new MySqlCommand(q4,conn);
                conn.Open();
                Int32 count = Convert.ToInt32(cmd4.ExecuteScalar());
                conn.Close();
                if (count != 0)
                {
                    string q0 = "select max(_date) maxDate from in_out where uid='" + inside.empId.ToString() + "' limit 1";
                    MySqlCommand cmd0 = new MySqlCommand(q0, conn);
                    string LastTrsnDate = "";
                    conn.Open();
                    MySqlDataReader rdr0 = cmd0.ExecuteReader();
                    while (rdr0.Read())
                    {
                        LastTrsnDate = rdr0.GetString("maxDate");
                    }
                    conn.Close();
                    //getting in-out status
                    string query = "select inout_status from nworksuser where uid='" + inside.empId + "';";
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);
                    string status = command.ExecuteScalar().ToString();
                    conn.Close();

                    if (Convert.ToDateTime(LastTrsnDate).ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        if (status == "Inside")
                        {
                            return string.Format("You are already in!");
                        }
                        else
                        {
                            string Query = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviceID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + inside.empId + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + "00:00:00" + "','" + inside.qrValue + "','" + inside.latitude + "','" + inside.longitude + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + inside.DeviceID.ToString() + "','" + inside.Location.ToString() + "','"+inside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                            MySqlCommand cmd = new MySqlCommand(Query, conn);
                            MySqlDataReader rdr;
                            conn.Open();
                            rdr = cmd.ExecuteReader();
                            conn.Close();

                            string Query1 = "update nworksuser set inout_status='Inside' where uid='" + inside.empId + "';";
                            conn.Open();
                            MySqlCommand cmd1 = new MySqlCommand(Query1, conn);
                            MySqlDataReader rdr1;
                            rdr1 = cmd1.ExecuteReader();
                            conn.Close();

                            return string.Format("You come IN suucessfully!");
                        }
                    }
                    else
                    {
                        if (status == "Inside")
                        {                            
                            string Query = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviceID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + inside.empId + "','00:00:00','" + Convert.ToDateTime(LastTrsnDate).AddDays(1).Date.AddSeconds(-17842).ToString("dd-MM-yyyy HH:mm:ss") + "','nWorks Technologies (India) Pvt. Ltd., 206 Garden Plaza, Rahatani, Pune, Maharashtra, 411 017, INDIA','" + inside.latitude + "','" + inside.longitude + "','" + Convert.ToDateTime(LastTrsnDate).ToString("yyyy-MM-dd") + "','" + inside.DeviceID.ToString() + "','" + inside.Location.ToString() + "','"+inside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                            MySqlCommand cmd = new MySqlCommand(Query, conn);
                            MySqlDataReader rdr;
                            conn.Open();
                            rdr = cmd.ExecuteReader();
                            conn.Close();

                            string q2 = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviceID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + inside.empId + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + "00:00:00" + "','" + inside.qrValue + "','" + inside.latitude + "','" + inside.longitude + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + inside.DeviceID.ToString() + "','" + inside.Location.ToString() + "','"+inside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
                            MySqlDataReader rdr2;
                            conn.Open();
                            rdr2 = cmd2.ExecuteReader();
                            conn.Close();

                            string q3 = "update nworksuser set inout_status='Inside' where uid='" + inside.empId + "';";
                            conn.Open();
                            MySqlCommand cmd3 = new MySqlCommand(q3, conn);
                            MySqlDataReader rdr3;
                            rdr3 = cmd3.ExecuteReader();
                            conn.Close();

                            return string.Format("You come IN suucessfully!");

                        }
                        else
                        {
                            string Query = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviceID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + inside.empId + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + "00:00:00" + "','" + inside.qrValue + "','" + inside.latitude + "','" + inside.longitude + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + inside.DeviceID.ToString() + "','" + inside.Location.ToString() + "','"+inside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                            MySqlCommand cmd = new MySqlCommand(Query, conn);
                            MySqlDataReader rdr;
                            conn.Open();
                            rdr = cmd.ExecuteReader();
                            conn.Close();

                            string Query1 = "update nworksuser set inout_status='Inside' where uid='" + inside.empId + "';";
                            conn.Open();
                            MySqlCommand cmd1 = new MySqlCommand(Query1, conn);
                            MySqlDataReader rdr1;
                            rdr1 = cmd1.ExecuteReader();
                            conn.Close();

                            return string.Format("You come IN suucessfully!");
                        }
                    }
                }
                else
                {
                    string q2 = "insert into in_out(uid,inTime,outTime,qrValue,latitude,longitude,_date,DeviveID,Location,DistanceFromOrigin,IsDeviceChanged) values('" + inside.empId + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + "00:00:00" + "','" + inside.qrValue + "','" + inside.latitude + "','" + inside.longitude + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + inside.DeviceID.ToString() + "','" + inside.Location.ToString() + "','"+inside.distanceFromOrigin.ToString()+"','"+IsDeviceChanged+"');";
                    MySqlCommand cmd2 = new MySqlCommand(q2, conn);
                    MySqlDataReader rdr2;
                    conn.Open();
                    rdr2 = cmd2.ExecuteReader();
                    conn.Close();

                    string Query1 = "update nworksuser set inout_status='Inside' where uid='" + inside.empId + "';";
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(Query1, conn);
                    MySqlDataReader rdr1;
                    rdr1 = cmd1.ExecuteReader();
                    conn.Close();

                    return string.Format("You come IN suucessfully!" );
                }               
                
            }
            catch (Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
       
    }
}
