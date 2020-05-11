using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class Item
    {
        public int ItemId{get;set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual ItemCategory Category { get; set; }
        public double Price { get; set; }
        public double DiscountAmount { get; set; }
        public double DiscountPercent { get; set; }
        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }
        public string Image { get; set; }

    }
}