using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class SendOTPController : ApiController
    {
        MySqlConnection conn = new MySqlConnection(Constants.conn);

        [HttpPost]
        public string send_otp(getOTP obj)
        {
            string otp = string.Empty;
            try
            {
                //OTP Generation
                string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";

                string characters = numbers;

                characters += alphabets + small_alphabets + numbers;

                int length = 7;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;
                }
                //

                //To make OTP one time, store it in table while its not used.
                string q0 = "select * from recoverpwdstatus where uid='" + obj.uid + "';";
                MySqlCommand cmd0 = new MySqlCommand(q0,conn);
                conn.Open();
                Int32 count = Convert.ToInt32(cmd0.ExecuteScalar());
                conn.Close();
                if(count>0)
                {
                    string q2 = "update recoverpwdstatus set guidcode='" + otp + "' where uid='" + obj.uid + "';";
                    MySqlCommand cmd2 = new MySqlCommand(q2,conn);
                    conn.Open();
                    MySqlDataReader rdr2 = cmd2.ExecuteReader();
                    conn.Close(); 
                }
                else
                {
                    string q = "insert into recoverpwdstatus values ('" + obj.uid.ToString() + "','" + otp + "');";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    conn.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    conn.Close();
                }
                
                //

                //Get user full name
                string  userFullname = "";
                string query = "select fname,lname from nworksuser where uid='" + obj.uid + "';";
                MySqlCommand cmd1 = new MySqlCommand(query, conn);
                MySqlDataReader rdr1;
                conn.Open();
                rdr1 = cmd1.ExecuteReader();
                while (rdr1.Read())
                {
                    userFullname = rdr1.GetString("fname") + " " + rdr1.GetString("lname");
                }
                conn.Close();
                //

                //Send OTP via email
                NetworkCredential login;
                SmtpClient client;
                MailMessage msg;
                login = new NetworkCredential("leaves@nworks.co", "password");
                client = new SmtpClient("smtp.1and1.com");
                client.Port = Convert.ToInt32(25);
                client.EnableSsl = true;
                client.Credentials = login;

                msg = new MailMessage { From = new MailAddress("leaves@nworks.co", "nWorks Employee", Encoding.UTF8) };
                msg.To.Add(new MailAddress(obj.email));//Email to
                msg.Subject = "Recover LeaveApp Password";//Subject            
                string strBody = string.Empty;
                strBody += "<html><head></head><body><h4>Hi " + userFullname + ",\n We got a request to reset your LeaveApp password.\n To reset your password use following One Time Password. If you did not forget your password, please ignore this email.</h4>\n\n<h1>"+otp+"\n\n</h1>";
                strBody += Environment.NewLine;
                strBody += "<br/>Thanks.";

                msg.Body = strBody;

                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                System.Diagnostics.Debug.WriteLine("Email is going to sent-----");
                client.Send(msg);
                System.Diagnostics.Debug.WriteLine("Email is sent-----");
                //return string.Format("Request is successfull...! Email sent!");
                return otp;

            }
            catch (Exception ex)
            {
                return string.Format("Email sending failed! Please Try Again!");
            }
            //
        }
    }
}
