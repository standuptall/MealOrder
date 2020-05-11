using BillMealMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BillMealMVC
{
    public class Utils
    {
        public static double GetPrice(Item item)
        {

            var newprice = item.Price - item.DiscountAmount;
            newprice = (item.DiscountPercent > 0) ? item.Price * (1 - (item.DiscountPercent / 100)) : newprice;
            newprice = Math.Round(newprice, 2);
            return newprice;

        }
    }
}