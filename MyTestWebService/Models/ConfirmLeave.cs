using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class ConfirmLeave
    {
        public string userId { get; set; }
        public string selectedDate { get; set; }
        public string requestedAs { get; set; }
    }
}

