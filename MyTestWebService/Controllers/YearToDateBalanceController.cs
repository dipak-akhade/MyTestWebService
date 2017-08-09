using MySql.Data.MySqlClient;
using MyTestWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTestWebService.Controllers
{
    public class YearToDateBalanceController : ApiController
    {
         MySqlConnection conn = new MySqlConnection(Constants.conn);
        // on Month selection, this ws will fire

         [HttpPost]
         public float userLeaveStatus()
         {
             return 2;
         }
    }
}
