using BillMealMVC.Model;
using BillMealMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public static CartHead GetCart(MealContext context,HttpRequestBase request,HttpResponseBase response)
        {
            var cook = request.Cookies.Get("meal_id");
            if (cook != null)
            {
                var cart = context.Cart.Where(c => c.meal_id == cook.Value)
                    .Include(c => c.Items)
                    .FirstOrDefault();
                if (cart.Closed)
                    response.Cookies["meal_id"].Expires = DateTime.Now.AddDays(-1);
                return cart;
            }
            return null;
        }
        public static string FormatPrice(double price)
        {
            return Math.Round(price, 2).ToString("C");
        }
    }
}