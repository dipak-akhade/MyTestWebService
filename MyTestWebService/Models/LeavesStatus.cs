using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class LeavesStatus
    {
        public int totalLeaves { get; set; }
        public int totalHolidays { get; set; }
        public double balanceLeaves { get; set; }
        public double balanceHolidays { get; set; }
        public List<Leaves> leaves { get; set; }
        public List<Holidays> holidays { get; set; }
    }

    public class Holidays
    {
        public string date { get; set; }
        public string weekday { get; set; }
    }

    public class Leaves
    {
        public string date { get; set; }
        public string weekday { get; set; }
        public string leaveType { get; set; }
    }
}