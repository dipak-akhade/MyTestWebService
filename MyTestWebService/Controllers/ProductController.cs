using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyTestWebService.Models;
using System.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using NodaTime;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MyTestWebService.Controllers
{


    public class ProductController : ApiController
    {


        [HttpGet]
        async public void UserLogin()
        {
           


            //DateTime timeUtc = DateTime.UtcNow;
            //Debug.WriteLine(timeUtc.ToString());
            //try
            //{
            //    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            //    return string.Format("IST :" + cstTime.TimeOfDay.ToString() + " Now to Universal :" + DateTime.Now.ToUniversalTime().ToString()+" UTC :"+timeUtc.ToString());
            //    return string.Format(cstTime.TimeOfDay.ToString().Substring(0, 8));

            //}
            //catch (TimeZoneNotFoundException)
            //{
            //    return string.Format("The registry does not define the Central Standard Time zone.");
            //}
            //catch (InvalidTimeZoneException)
            //{
            //    return string.Format("Registry data on the Central Standard Time zone has been corrupted.");
            //}
        }

    }
}
