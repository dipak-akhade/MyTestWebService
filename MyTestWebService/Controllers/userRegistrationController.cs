using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyTestWebService.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


namespace MyTestWebService.Controllers
{
    public class userRegistrationController : ApiController
    {
        private static string Key = "hhjnksjcojddocjcnndsaopskssdsdsw";
        private static string IV = "jdishikchjohdjvs";
        MySqlConnection conn = new MySqlConnection(Constants.conn);
        [HttpPost]
        public string userRegistration(userRegistration userRegister)
        {
            try
            {
                string Query1 = "select count(*) from nworksuser where username='" + userRegister.username + "' or uid='" + userRegister.uid + "';";
                MySqlCommand cmd = new MySqlCommand(Query1,conn);
                conn.Open();
                Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                cmd.Dispose();
                if (count != 0)
                {
                    return string.Format("Userid or Username is already exist!");
                }
                else
                {
                    string Query = "insert into nWorksUser values('" + userRegister.uid + "','" + userRegister.fname + "','" + userRegister.mname + "','" + userRegister.lname + "','" + userRegister.dob + "','" + userRegister.gender + "','" + userRegister.desg + "','" + userRegister.hire_date + "','" + userRegister.username + "','" + Encrypt(userRegister.password) + "','" + userRegister.addressLine1 + "','" + userRegister.addressLine2 + "','" + userRegister.state + "','" + userRegister.city + "','" + userRegister.pincode + "','" + userRegister.country + "','" + userRegister.mobileNo + "','" + userRegister._type + "','" + userRegister.userActive + "','" + userRegister.leaveDate + "','" + userRegister.anniversary + "','" + userRegister.email + "','"+userRegister.inout_status+"');";
                    MySqlCommand MyCommand = new MySqlCommand(Query, conn);
                    MySqlDataReader MyReader;
                    conn.Open();
                    MyReader = MyCommand.ExecuteReader();
                    conn.Close();

                    string q = "insert into additional_earned_leaves values('" + userRegister.uid + "','0','0','" + DateTime.Today.Year.ToString() + "');";
                    MySqlCommand command = new MySqlCommand(q, conn);
                    MySqlDataReader rdr;
                    conn.Open();
                    rdr = command.ExecuteReader();
                    conn.Close();

                    return string.Format("Registration Successful!");
                }
            }
            catch (Exception ex)
            {
                return string.Format(ex.ToString());
            }
        }
        public static string Decrypt(string encrypted)
        {
            byte[] encryptedbytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);

            return System.Text.ASCIIEncoding.ASCII.GetString(secret);
        }
        private static string Encrypt(string text)
        {
            byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }
    }
}
