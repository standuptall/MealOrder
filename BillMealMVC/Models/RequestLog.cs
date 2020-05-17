using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class RequestLog
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}