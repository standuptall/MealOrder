using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int? NumeroProdotti { get; set; }
    }
}