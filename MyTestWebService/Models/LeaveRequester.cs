using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class LeaveRequester
    {
        public string userId { get; set; }
        public string userFullName { get; set; }
        public string requestedOn { get; set; }
    }
}