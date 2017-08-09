using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class ExtraUsersParameter
    {
        public float Rolled_Over_From_Previous_Year { get; set; }
        public float Additional_Earned_in_Current_Year { get; set; }
        public float Without_Pay_in_Current_Year { get; set; }
        public string uid { get; set; }
    }
}