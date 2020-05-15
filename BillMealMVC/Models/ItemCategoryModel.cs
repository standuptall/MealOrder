using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class ItemCategoryModel
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int NumberOfProducts { get; set; }
    }
}