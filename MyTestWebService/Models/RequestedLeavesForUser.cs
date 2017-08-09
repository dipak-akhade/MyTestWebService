using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class RequestedLeavesForUser
    {
        public string leavedate { get; set; }
        public string requestedAs { get; set; }
        public bool isWithPay { get; set; }
        public bool isAccepted { get; set; }
    }
}