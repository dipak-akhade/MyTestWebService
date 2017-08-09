using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class comingInANDgoingOut
    {
        public string empId { get; set; }
        public string DeviceID { get; set; }
        public string Location { get; set; }
        public string qrValue { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string distanceFromOrigin { get; set; }

    }
}