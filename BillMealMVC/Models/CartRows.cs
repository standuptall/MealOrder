using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class CartRows
    {
        [Key]
        public int CartRowId { get; set; }
        public int? CartId { get; set; }
        public CartHead CartHead { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public double Quantity { get; set; }

    }
}