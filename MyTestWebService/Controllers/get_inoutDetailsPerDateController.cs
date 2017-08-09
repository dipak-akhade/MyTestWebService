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
    public class get_inoutDetailsPerDateController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public inoutDetailsPerDate get_inoutDetailsPerDate(get_inoutDetails obj)
        {
            
            inoutDetailsPerDate data = new inoutDetailsPerDate();

            Debug.WriteLine(string.Format("{0:yyyy-MM-dd}", obj.choosedDate.ToString())+"   "+obj.userid.ToString());
            Debug.WriteLine(" Universal Time :" +Convert.ToDateTime(obj.choosedDate).ToUniversalTime().ToString("yyyy-MM-dd"));

            string q4 = "select distinct inTime,isdevicechanged,deviceid,location,distancefromorigin from in_out where inTime != '00:00:00' and uid='" + obj.userid + "' and _date='" + Convert.ToDateTime(obj.choosedDate).ToString("yyyy-MM-dd") + "';";
            string q5 = "select distinct outTime,isdevicechanged,deviceid,location,distancefromorigin from in_out where outTime != '00:00:00' and uid='" + obj.userid + "' and _date='" + Convert.ToDateTime(obj.choosedDate).ToString("yyyy-MM-dd") + "';";

            MySqlCommand cmd4 = new MySqlCommand(q4, conn);
            MySqlCommand cmd5 = new MySqlCommand(q5, conn);

            List<inTimes> MYinT = new List<inTimes>();
            List<outTimes> MYoutT = new List<outTimes>();

            MySqlDataReader rdr4;
            MySqlDataReader rdr5;

            conn.Open();
            rdr4 = cmd4.ExecuteReader();
            while (rdr4.Read())
            {
                inTimes A = new inTimes();
                //MYinT.Add(getConvert(rdr4.GetString("inTime").Substring(10, 8)));
                //inT.Add((rdr4.getString("inTime").Substring(10,8)));
                A.INTIMES = getConvert(rdr4.GetString("inTime").Substring(10, 8));
               A.deviceid = rdr4.GetString("deviceid");
                A.location = rdr4.GetString("location");
                if (rdr4.GetFloat("DistanceFromOrigin") > 100.000 || rdr4.GetInt16("IsDeviceChanged") == 1)
                {
                    Debug.WriteLine(rdr4.GetFloat("DistanceFromOrigin").ToString());
                    A.IsDevice_or_LocationChanged = "Red";
                }
                else
                    A.IsDevice_or_LocationChanged = "White";
                MYinT.Add(A);
            }
            conn.Close();

            conn.Open();
            rdr5 = cmd5.ExecuteReader();
            while (rdr5.Read())
            {
                outTimes B = new outTimes();
                //MYoutT.Add(getConvert(rdr5.GetString("outTime").Substring(10, 8)));
                B.OUTTIMES = getConvert(rdr5.GetString("outTime").Substring(10, 8));
                B.deviceid = rdr5.GetString("deviceid");
                B.location = rdr5.GetString("location");
                if (rdr5.GetFloat("DistanceFromOrigin") > 100.000 || rdr5.GetInt16("IsDeviceChanged") == 1)
                {
                    Debug.WriteLine(rdr5.GetFloat("DistanceFromOrigin").ToString());
                    B.IsDevice_or_LocationChanged = "Red";
                }
                else
                    B.IsDevice_or_LocationChanged = "White";
                MYoutT.Add(B);
            }
            conn.Close();

            data.inTimes = MYinT;
            data.outTimes = MYoutT;

            return data;
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
