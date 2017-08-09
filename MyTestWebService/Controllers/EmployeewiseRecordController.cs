using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class EmployeewiseRecordController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        List<EmployeeWiseData> MainData = new List<EmployeeWiseData>();

        [HttpPost]
        public List<EmployeeWiseData> getEmployeewiseDetails(getEmployeewiseRecord obj)
        {

            for (int i = 0; i < obj.uid.Count; i++)
            {
                DateTime forDate = obj.fromdate;

                List<EmployeeData> ListOfEmployeeData = new List<EmployeeData>();

                EmployeeWiseData mainObject = new EmployeeWiseData();

                string q0 = "select fname,lname from nworksuser where uid='" + obj.uid[i].ToString() + "';";

                MySqlCommand cmd0 = new MySqlCommand(q0, conn);

                conn.Open();

                MySqlDataReader rdr0 = cmd0.ExecuteReader();

                while (rdr0.Read())
                {
                    mainObject.EmployeeName = rdr0.GetString("fname") + " " + rdr0.GetString("lname");//Employee name
                }
                conn.Close();

                while (forDate <= obj.todate)
                {

                    EmployeeData objEmployeeData = new EmployeeData();

                    Debug.WriteLine("For date : " + forDate.ToString("yyyy-MM-dd"));

                    string q = "select * from in_out where uid='" + obj.uid[i].ToString() + "' and _date = '" + forDate.ToString("yyyy-MM-dd") + "';";
                    string q1 = "select count(*) from in_out where uid='" + obj.uid[i].ToString() + "' and _date = '" + forDate.ToString("yyyy-MM-dd") + "';";
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(q1, conn);
                    Int32 c = Convert.ToInt32(cmd1.ExecuteScalar());
                    Debug.WriteLine("Count :" + c);

                    if (c > 0)
                    {
                        conn.Close();
                        objEmployeeData.date = forDate.ToString("dd-MMM-yyyy");     //date
                        objEmployeeData.weekday = forDate.DayOfWeek.ToString();     //weekday
                        objEmployeeData.uid = obj.uid[i].ToString();    //uid
                        string q2 = "select * from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and inTime!='00:00:00' order by inout_id asc limit 1";//get in time
                        string query2 = "select count(*) from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and inTime!='00:00:00' order by inout_id asc limit 1";//get in time
                        string q3 = "select * from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and outTime!='00:00:00' order by inout_id desc limit 1";//get out time
                        string query3 = "select count(*) from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and outTime!='00:00:00' order by inout_id desc limit 1";//get out time
                        MySqlCommand cmd2 = new MySqlCommand(q2, conn);
                        MySqlCommand command2 = new MySqlCommand(query2, conn);
                        MySqlCommand cmd3 = new MySqlCommand(q3, conn);
                        MySqlCommand command3 = new MySqlCommand(query3, conn);
                        MySqlDataReader rdr2, rdr3;

                        conn.Open();
                        Int32 inCount = Convert.ToInt32(command2.ExecuteScalar());
                        conn.Close();
                        conn.Open();
                        Int32 outCount = Convert.ToInt32(command3.ExecuteScalar());
                        conn.Close();


                        if(inCount!=0)
                        {
                            conn.Open();
                            rdr2 = cmd2.ExecuteReader();
                            string firstIn = "";

                            while (rdr2.Read())
                            {
                                firstIn = rdr2.GetString("inTime").Substring(10,8);        //fin time
                            }
                            Debug.WriteLine("........>intime"+firstIn);
                            objEmployeeData.In = getConvert(firstIn);
                            conn.Close();
                        }

                        if(outCount!=0)
                        {
                            conn.Open();
                            rdr3 = cmd3.ExecuteReader();
                            string lastOut = "";
                            while (rdr3.Read())
                            {
                                lastOut = rdr3.GetString("outTime").Substring(10, 8);      //Lout time
                            }
                            objEmployeeData.Out = getConvert(lastOut);
                            conn.Close();
                        }

                        
                        //getting total time
                        string q4 = "select inTime from in_out where inTime != '00:00:00' and uid='" + obj.uid[i].ToString() + "' and _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        string q5 = "select outTime from in_out where outTime != '00:00:00' and uid='" + obj.uid[i].ToString() + "' and _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        MySqlCommand cmd4 = new MySqlCommand(q4, conn);
                        MySqlCommand cmd5 = new MySqlCommand(q5, conn);

                        List<string> inT = new List<string>();
                        List<string> outT = new List<string>();

                        MySqlDataReader rdr4;
                        MySqlDataReader rdr5;

                        conn.Open();
                        rdr4 = cmd4.ExecuteReader();
                        while (rdr4.Read())
                        {
                            inT.Add((rdr4.GetString("inTime").Substring(10, 8)));
                        }
                        conn.Close();

                        conn.Open();
                        rdr5 = cmd5.ExecuteReader();
                        while (rdr5.Read())
                        {
                            outT.Add((rdr5.GetString("outTime").Substring(10, 8)));
                        }
                        conn.Close();

                        int count = inT.Count - outT.Count;
                        Debug.WriteLine("in time count : " + inT.Count.ToString());
                        Debug.WriteLine("out time count : " + outT.Count.ToString());                        


                        TimeSpan TotalTime = TimeSpan.FromSeconds(1);
                        if (count == -1 || count == 0)
                        {
                            int cnt = inT.Count;
                            for (int k = 0; k < cnt; k++)
                            {
                                TimeSpan span = TimeSpan.Parse(outT[k]) - TimeSpan.Parse(inT[k]);
                                TimeSpan duration = span.Duration();
                                TotalTime = TotalTime + duration;
                            }
                        }
                        else if (count == 1)
                        {
                            //need to improve this code
                            int cnt = outT.Count;
                            for (int k = 0; k < cnt; k++)
                            {
                                TimeSpan span = TimeSpan.Parse(outT[k]) - TimeSpan.Parse(inT[k]);
                                TimeSpan duration = span.Duration();
                                TotalTime = TotalTime + duration;
                            }
                        }
                        objEmployeeData.totalInTime = TotalTime.ToString();        //total time

                        ListOfEmployeeData.Add(objEmployeeData);


                        //If distance from origin is greater than 40 meter then send Address with how much distance long from origin
                        string myQuery1 = "select * from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "'";

                        conn.Open();
                        MySqlCommand myCommand1 = new MySqlCommand(myQuery1, conn);
                        MySqlDataReader myRdr1 = myCommand1.ExecuteReader();


                        while (myRdr1.Read())
                        {
                            if (myRdr1.GetFloat("DistanceFromOrigin") > 100.000 || myRdr1.GetInt16("IsDeviceChanged") == 1)
                            {
                                Debug.WriteLine(myRdr1.GetFloat("DistanceFromOrigin").ToString());
                                objEmployeeData.Is_Loc_Device_Changed = "Red";
                            }                          
                            else
                                objEmployeeData.Is_Loc_Device_Changed = "White";


                        }
                        conn.Close();
                        //If IsDeviceChanged is true(1) send device id 


                        conn.Open();// 
                    }
                    conn.Close();
                    forDate = forDate.AddDays(1);
                }

                mainObject.employeeWiseData = ListOfEmployeeData;

                MainData.Add(mainObject);
            }

            return MainData;
        }

        public string getConvert(string myUtc)
        {
            DateTime timeUtc = DateTime.Parse(myUtc);

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, istZone);

            return string.Format(istTime.TimeOfDay.ToString().Substring(0, 8));
        }
    }
}
