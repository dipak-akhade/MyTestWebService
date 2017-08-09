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
    public class CancelReqLeaveController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string CancelReqDate(CancelReqLeaves obj)
        {
            try
            {
                string q0 = "delete from leaves where empno='" + obj.uid + "' and leavedate='" + obj.dateToBeCancel.ToString("yyyy-MM-dd") + "';";
                MySqlCommand cmd0 = new MySqlCommand(q0, conn);
                conn.Open();
                int rowAffected = cmd0.ExecuteNonQuery();
                MySqlDataReader rdr0 = cmd0.ExecuteReader();
                conn.Close();

                if (rowAffected > 0)
                {
                    //get email and fullname address of user

                    string userEmail = "", userFullname = "";
                    string query = "select email,fname,lname from nworksuser where uid='" + obj.uid + "';";
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
                    //string PrawalGupta = "dipak.a.akhade9192@gmail.com";
                    msg = new MailMessage { From = new MailAddress("leaves@nworks.co", "nWorks Employee", Encoding.UTF8) };
                    msg.To.Add(new MailAddress(userEmail));//Email to
                    msg.Subject = "Leave/Holiday Cancellation.";//Subject
                    msg.CC.Add(new MailAddress(PrawalGupta));//Email to CC
                                                             //msg.CC.Add(new MailAddress(ParulGupta));//Email to CC
                    string strBody = string.Empty;
                    strBody += "<html><head></head><body><h2>Dear </h2><p><b>" + obj.uid + " " + userFullname + ",</b> " + " Your requested leave/holiday " + obj.dateToBeCancel.ToString("dd-MMM-yyyy") + " was accepted earlier but for any reason it is cancelled now. Please contact your manager for more details.</p>";
                    strBody += Environment.NewLine;

                    strBody += "<br/>Thanks.";

                    msg.Body = strBody;

                    msg.BodyEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;
                    msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    System.Diagnostics.Debug.WriteLine("Email is going to sent-----");
                    client.Send(msg);

                    return string.Format("Cancellation Successfull! Email sent!");
                }
                else
                {
                    return string.Format("No such record found!");

                }
            }
            catch (Exception Ex)
            {
                return string.Format("Cancellation Successfull! Email Sending Failure!");
            }
        }
    }
}

