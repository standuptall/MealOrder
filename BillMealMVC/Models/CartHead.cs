﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class CartHead
    {
        [Key]
        public int CartId { get; set; }
        public string meal_id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CartRows> Items { get; set; }
        public bool Closed { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int DeliveryHour { get; set; }
        public int DeliveryMinute { get; set; }
    }
}