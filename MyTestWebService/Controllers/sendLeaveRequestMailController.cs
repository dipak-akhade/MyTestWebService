using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class sendLeaveRequestMailController : ApiController
    {

        [HttpPost]
        public string RequestTimeOff(List<LeaveRequestData> LRdata)
        {
            MySqlConnection conn = new MySqlConnection(Constants.conn);
            //get user id from object LRdata
            string userid = "";
            for (int i = 0; i < LRdata.Count; i++)
            {
                userid = LRdata[i].uid;
            }
            //

            //Check if any date is already  requested
            string q00 = "select * from temporaryrequesteddates where uid='"+userid+"';";
            MySqlCommand cmd00 = new MySqlCommand(q00, conn);
            conn.Open();
            List<string> alreadyReqDates = new List<string>();
            MySqlDataReader rdr00 = cmd00.ExecuteReader();
            while (rdr00.Read())
            {
                for (int i = 0; i < LRdata.Count; i++)
                {
                    if (rdr00.GetDateTime("dates") ==Convert.ToDateTime(LRdata[i].requestedDate))
                    {
                        alreadyReqDates.Add(LRdata[i].requestedDate);
                    }
                }
            }
            conn.Close();
            //

            //Check if any date is already  taken
            string q01 = "select * from leaves where empno='"+userid+"';";
            MySqlCommand cmd01 = new MySqlCommand(q01, conn);
            conn.Open();
            List<string> alreadyTakenDates = new List<string>();
            MySqlDataReader rdr01 = cmd01.ExecuteReader();
            while (rdr01.Read())
            {
                for (int i = 0; i < LRdata.Count; i++)
                {
                    if (rdr01.GetString("leavedate") == LRdata[i].requestedDate)
                    {
                        alreadyTakenDates.Add(LRdata[i].requestedDate);
                    }
                }
            }
            conn.Close();
            //


            //get email and fullname address of user
            string userEmail = "", userFullname = "";
            string query = "select email,fname,lname from nworksuser where uid='" + userid + "';";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader rdr;
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                userEmail = rdr.GetString("email");
                userFullname = rdr.GetString("fname") + " " + rdr.GetString("lname");
            }
            conn.Close();
            //
            if (alreadyReqDates.Count==0 && alreadyTakenDates.Count==0)
            {
                //Insert requested dates in temporary table 
                try
                {
                    for (int i = 0; i < LRdata.Count; i++)
                    {
                        string q = "insert into temporaryrequesteddates values('" + LRdata[i].uid + "','" + LRdata[i].requestedDate + "','" + LRdata[i].requestedAs + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                        MySqlCommand cmd1 = new MySqlCommand(q, conn);
                        conn.Open();
                        MySqlDataReader rdr1;
                        rdr1 = cmd1.ExecuteReader();
                        conn.Close();
                    }
                    //

                    //Send Email to Requester and Higher Authority
                    NetworkCredential login;
                    SmtpClient client;
                    MailMessage msg;
                    login = new NetworkCredential("leaves@nworks.co", "password");
                    client = new SmtpClient("smtp.1and1.com");
                    client.Port = Convert.ToInt32(25);
                    client.EnableSsl = true;
                    client.Credentials = login;

                    string PrawalGupta = "prawal.gupta@nworks.co";
                    msg = new MailMessage { From = new MailAddress("leaves@nworks.co", "nWorks Employee", Encoding.UTF8) };
                    msg.To.Add(new MailAddress(userEmail));//Email to
                    msg.Subject = "Leave/Holiday request by " + userid + " : " + userFullname;//Subject
                    msg.CC.Add(new MailAddress(PrawalGupta));//Email to CC
                                                             //msg.CC.Add(new MailAddress(ParulGupta));//Email to CC
                    string strBody = string.Empty;
                    strBody += "<html><head></head><body><h2>Dear Prawal GUPTA,</h2><p><b>" + userid + " " + userFullname + "</b> " + " has requested leaves/holidays on the following dates :</p>";
                    strBody += Environment.NewLine;

                    int j = 1;

                    for (int k = 0; k < LRdata.Count; k++)                  // here "lstDate" is name of your list where you store all date.
                    {
                        strBody += j + ". " + Convert.ToDateTime(LRdata[k].requestedDate).ToString("dd-MMM-yyyy") + ", " + Convert.ToDateTime(LRdata[k].requestedDate).DayOfWeek + "  : " + LRdata[k].requestedAs + " <br>";
                        j++;
                    }
                    strBody += "<br/>Thanks.";

                    msg.Body = strBody;

                    msg.BodyEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;
                    msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    System.Diagnostics.Debug.WriteLine("Email is going to sent-----");
                    client.Send(msg);
                    System.Diagnostics.Debug.WriteLine("Email is sent-----");
                    return string.Format("Request is successfull...! Email sent!");
                }
                catch (Exception ex)
                {
                    return string.Format("Request is successfull...! Email sending failed!");
                }
            }
            else if(alreadyReqDates.Count>0 )
            {
                string result = "";
                for(int i=0;i<alreadyReqDates.Count;i++)
                {
                    result += alreadyReqDates[i] + ", ";
                }
                               
                return string.Format(result+ "Date/s already requested!");

            }
            else
            {
                string result = "";
                for (int i = 0; i < alreadyTakenDates.Count; i++)
                {
                    result += alreadyTakenDates[i] + ", ";
                }

                return string.Format(result + "Date/s already taken!");

            }
        }
        
    }
}
