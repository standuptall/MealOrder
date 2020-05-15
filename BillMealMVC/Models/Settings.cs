using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string FooterDescription { get; set; }
        public string ApplicationPath { get; set; }
        public string UserEmail { get; set; }
        public string PasswordEmail { get; set; }
    }
}