using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWebService.Models
{
    public class CancelReqLeaves
    {
        public string uid { get; set; }
        public DateTime dateToBeCancel { get; set; }
    }
}