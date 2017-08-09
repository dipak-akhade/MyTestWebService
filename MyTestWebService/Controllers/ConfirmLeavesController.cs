using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;
using System.Text;
using System.Net.Mail;

namespace MyTestWebService.Controllers
{
    public class ConfirmLeavesController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string LeaveConfirmation(model_ConfirmLeaves data)
        {
            model_ConfirmLeaves obj = new model_ConfirmLeaves();
            obj.userId = data.userId;
            List<model_Dates> objDates = new List<model_Dates>();
            objDates = data.Dates;
            List<model_DeselectedDates> objDeselectedDates = new List<model_DeselectedDates>();
            objDeselectedDates = data.deselectedDates;

            try
            {
                string q1 = "",  uid = "";
                
                uid = obj.userId;

                //get email and fullname address of user
                string userEmail = "", userFullname = "";
                string query = "select email,fname,lname from nworksuser where uid='" + uid + "';";
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


                //send email of action
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
                msg.Subject = "Leave/Holiday request by " + uid + " : " + userFullname;//Subject
                msg.CC.Add(new MailAddress(PrawalGupta));//Email to CC
                string strBody = string.Empty;

                if (objDates.Count==0)
                {
                    strBody += "<html><head></head><body><h2>Dear <p><b> " + uid + " " + userFullname + ",</b></h2> " + " Your no one leaves/holidays are accepted.\n</p>";
                    strBody += Environment.NewLine;
                    strBody += "<br/>Thanks.";

                    clearTemporaryDates(uid);
                }
                else
                {
                    //Fixed the leaves
                    for (int i = 0; i < objDates.Count; i++)
                    {
                        q1 = "insert into leaves values('" + uid + "','" + objDates[i].selectedDate.ToString() + "','" + objDates[i].requestedAs.ToString() + "','" + objDates[i].isWithPay.ToString() + "');";
                        MySqlCommand command = new MySqlCommand(q1, conn);
                        conn.Open();
                        MySqlDataReader reader = command.ExecuteReader();
                        conn.Close();
                    }

                    strBody += "<html><head></head><body><h2>Dear <p><b> " + uid + " " + userFullname + ",</b></h2> " + " Your accepted leaves/holidays are as follows :\n</p>";
                    strBody += Environment.NewLine;

                    int j = 1;

                    for (int k = 0; k < objDates.Count; k++)                  // here "lstDate" is name of your list where you store all date.
                    {
                        strBody += j + ". " + Convert.ToDateTime(objDates[k].selectedDate).ToString("dd-MMM-yyyy") + ", " + Convert.ToDateTime(objDates[k].selectedDate).DayOfWeek + "  : " + objDates[k].requestedAs + " WithPay?("+objDates[k].isWithPay.ToString()+") <br>";
                        j++;
                    }
                    if (objDeselectedDates.Count != 0)
                    {
                        strBody += "<br><br><p>Your rejected leaves/holidays are as follows :\n</p>";
                        int m = 1;

                        for (int n = 0; n < objDeselectedDates.Count; n++)                  // here "lstDate" is name of your list where you store all date.
                        {
                            strBody += m + ". " + Convert.ToDateTime(objDeselectedDates[n].selectedDate).ToString("dd-MMM-yyyy") + ", " + Convert.ToDateTime(objDeselectedDates[n].selectedDate).DayOfWeek + "  : " + objDeselectedDates[n].requestedAs + " <br>";
                            m++;
                        }
                    }
                    strBody += "<br/>Thanks.";

                    clearTemporaryDates(uid);


                }
                msg.Body = strBody;

                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(msg);
                return string.Format("Action Performed! Email Sent!");
            }
            catch (Exception ex)
            {
                return string.Format("Action Performed! Email sending failed!");

            }
        }
        public void clearTemporaryDates(string uid)
        {
            //Remove dates from temporary table
            string q2 = "delete from temporaryrequesteddates where uid='" + uid + "';";
            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
            conn.Open();
            MySqlDataReader rdr2 = cmd2.ExecuteReader();
            conn.Close();
        }
    }
}
