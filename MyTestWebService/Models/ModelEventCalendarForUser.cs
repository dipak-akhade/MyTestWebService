using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class ModelEventCalendarForUser
    {
        public Boolean isAllow { get; set; }
        public string Occasion { get; set; }
        public string OccasionDate { get; set; }
        public string OccasionWeekDay { get; set; }
    }
}