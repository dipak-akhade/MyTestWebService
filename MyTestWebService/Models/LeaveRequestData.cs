using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class LeaveRequestData
    {
        public string uid { get; set; }
        public string requestedDate { get; set; }
        public string requestedAs { get; set; }
    }
}