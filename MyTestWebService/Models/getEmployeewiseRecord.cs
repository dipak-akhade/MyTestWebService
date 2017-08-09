using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class getEmployeewiseRecord
    {
        public List<string> uid { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
}