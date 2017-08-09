using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class DatewiseRecordController : ApiController
    {

        MySqlConnection conn = new MySqlConnection(Constants.conn);

        List<DateWiseData> MainData = new List<DateWiseData>();


        [HttpPost]

        public List<DateWiseData> getDatewiseRecord(getEmployeewiseRecord obj)
        {
            DateTime forDate = obj.fromdate;

            Debug.WriteLine("For date :"+ forDate.ToString());
            Debug.WriteLine("From  date :"+ obj.fromdate.ToString());
            
            while(forDate<=obj.todate)
            {
                DateWiseData datewisedata = new DateWiseData();

                datewisedata._Date = forDate.ToString("yyyy-MM-dd");//Date                

                List<DateData> listDateData = new List<DateData>();
                
                for(int i=0;i<obj.uid.Count;i++)
                {
                    DateData dateData = new DateData();                    

                    //first in time and out time
                    string q1 = "select * from in_out where uid='" + obj.uid[i].ToString() + "' and _date = '" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                    string q2 = "select count(*) from in_out where uid='" + obj.uid[i].ToString() + "' and _date = '" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                    conn.Open();
                    MySqlCommand cmd2 = new MySqlCommand(q2, conn);
                    Int32 c = Convert.ToInt32(cmd2.ExecuteScalar());
                    conn.Close();
                    Debug.WriteLine("Count :" + c);

                    if (c > 0)
                    {
                        Debug.WriteLine("For date individual :"+ forDate.ToString("dd-MMM-yyyy"));

                        dateData._date = forDate.ToString("dd-MMM-yyyy");//date
                        dateData.uid = obj.uid[i].ToString();//uid
                        dateData.weekday = forDate.DayOfWeek.ToString();//weekday
                        
                        string q0 = "select fname,lname from nworksuser where uid='" + obj.uid[i].ToString() + "';";
                        MySqlCommand cmd0 = new MySqlCommand(q0, conn);
                        MySqlDataReader rdr0;
                        conn.Open();
                        rdr0 = cmd0.ExecuteReader();
                        while (rdr0.Read())
                        {
                            dateData.employeeName = rdr0.GetString("fname") + " " + rdr0.GetString("lname");//Employee name
                        }
                        conn.Close();

                        string q3 = "select * from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and inTime!='00:00:00' order by inout_id asc limit 1";//get in time
                        string query3 = "select count(*) from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and inTime!='00:00:00' order by inout_id asc limit 1";//get in time
                        string q4 = "select * from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and outTime!='00:00:00' order by inout_id desc limit 1";//get out time
                        string query4 = "select count(*) from in_out where _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='" + obj.uid[i].ToString() + "' and outTime!='00:00:00' order by inout_id desc limit 1";//get out time
                        MySqlCommand cmd3 = new MySqlCommand(q3, conn);
                        MySqlCommand command3 = new MySqlCommand(query3, conn);
                        MySqlCommand cmd4 = new MySqlCommand(q4, conn);
                        MySqlCommand command4 = new MySqlCommand(query4, conn);
                        MySqlDataReader rdr3, rdr4;

                        conn.Open();
                        Int32 inCount = Convert.ToInt32(command3.ExecuteScalar());
                        conn.Close();
                        conn.Open();
                        Int32 outCount = Convert.ToInt32(command4.ExecuteScalar());
                        conn.Close();

                        if(inCount!=0)
                        {
                            conn.Open();
                            rdr3 = cmd3.ExecuteReader();
                            string firstIn = "";
                            while (rdr3.Read())
                            {
                                firstIn = rdr3.GetString("inTime").Substring(10, 8);       //fin time
                            }
                            Debug.WriteLine("Fin :" + firstIn);
                            dateData.fin = getConvert(firstIn);
                            conn.Close();
                        }
                        
                        if(outCount!=0)
                        {
                            conn.Open();
                            rdr4 = cmd4.ExecuteReader();
                            string firstOut = "";
                            while (rdr4.Read())
                            {
                                firstOut = rdr4.GetString("outTime").Substring(10, 8);     //fout time
                            }
                            dateData.fout = getConvert(firstOut);
                            conn.Close();
                        }

                        //total in time
                        string q5 = "select distinct inTime from in_out where inTime != '00:00:00' and uid='" + obj.uid[i].ToString() + "' and _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        string q6 = "select distinct outTime from in_out where outTime != '00:00:00' and uid='" + obj.uid[i].ToString() + "' and _date='" + forDate.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        MySqlCommand cmd5 = new MySqlCommand(q5, conn);
                        MySqlCommand cmd6 = new MySqlCommand(q6, conn);

                        List<string> inT = new List<string>();
                        List<string> outT = new List<string>();

                        MySqlDataReader rdr5;
                        MySqlDataReader rdr6;

                        conn.Open();
                        rdr5 = cmd5.ExecuteReader();
                        while (rdr5.Read())
                        {
                            inT.Add((rdr5.GetString("inTime").Substring(10,8)));
                        }
                        conn.Close();

                        conn.Open();
                        rdr6 = cmd6.ExecuteReader();
                        while (rdr6.Read())
                        {
                            outT.Add((rdr6.GetString("outTime").Substring(10,8)));
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
                        dateData.totalInTime = TotalTime.ToString();        //total time


                        string myQuery = "select * from in_out where _date='"+forDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and uid='"+obj.uid[i].ToString()+"';";
                        conn.Open();
                        MySqlCommand myCommand1 = new MySqlCommand(myQuery, conn);
                        MySqlDataReader myRdr1 = myCommand1.ExecuteReader();


                        while (myRdr1.Read())
                        {
                            if (myRdr1.GetFloat("DistanceFromOrigin") > 100.000 || myRdr1.GetInt16("IsDeviceChanged") == 1)
                            {
                                Debug.WriteLine(myRdr1.GetFloat("DistanceFromOrigin").ToString());
                                dateData.Is_Loc_Device_Changed = "Red";
                            }                            
                            else
                                dateData.Is_Loc_Device_Changed = "White";
                        }
                        conn.Close();
                        listDateData.Add(dateData);
                    }
                }

                datewisedata.dateData = listDateData;

                MainData.Add(datewisedata);

                forDate = forDate.AddDays(1);
            }

            return MainData;
        }

        public string getConvert(string myUtc)
        {

            DateTime timeUtc = DateTime.Parse(myUtc);

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, istZone);

            return string.Format(istTime.TimeOfDay.ToString().Substring(0,8));
        }
    }
}
