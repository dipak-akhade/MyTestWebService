using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class inoutDetailsPerDate
    {
        //sending info on the basis of userid and particular date
       public  List<inTimes> inTimes { get; set; }
       public  List<outTimes> outTimes { get; set; }
    }
    public class inTimes
    {
        public string INTIMES { get; set; }
        public string deviceid { get; set; }
        public string location { get; set; }
        public string IsDevice_or_LocationChanged { get; set; }//
    }
    public class outTimes
    {
        public string OUTTIMES { get; set; }
        public string deviceid { get; set; }
        public string location { get; set; }
        public string IsDevice_or_LocationChanged { get; set; }//
    }
    
}