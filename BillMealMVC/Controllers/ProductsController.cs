using BillMealMVC.Model;
using BillMealMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class ProductsController : Controller
    {
        MealContext context = new MealContext();
        public ActionResult Index(int? cat)
        {
            List<Item> items = null;
            if (cat != null && cat > 0)
            {
                items = context.Items.Where(c => c.CategoryId == cat).ToList();
            }
            else
                items = context.Items.ToList();
            return View(items);
        }

        public ActionResult AddToCart(int? id)
        {
            var ret = View("Index", context.Items.ToList());
            if (id == null)
                return ret;
            var item = context.Items.Where(c => c.ItemId == id).FirstOrDefault();
            if (item == null)
                return ret;
            var meal_id = Request.Cookies["meal_id"]?.Value;
            CartHead cartHead = null;
            if (string.IsNullOrEmpty(meal_id))
            {
                Guid g = Guid.NewGuid();
                meal_id = g.ToString();
                var cook = new HttpCookie("meal_id");
                cook.Expires = DateTime.Now.AddDays(1);
                cook.Value = meal_id;
                Response.Cookies.Add(cook);
                cartHead = new CartHead
                {
                    CreationDate = DateTime.Now,
                    meal_id = meal_id,
                    Items = new List<CartRows>()
                };
                context.Cart.Add(cartHead);
            }
            else
                cartHead = context.Cart
                    .Where(c => c.meal_id == meal_id)
                    .Include(c => c.Items)
                    .FirstOrDefault();
            if (cartHead == null)
                return ret;
            CartRows cartrow = null;
            cartrow = cartHead.Items?.Where(c => c.ItemId == item.ItemId).FirstOrDefault();
            if (cartrow == null)
            {
                cartrow = new CartRows
                {
                    Quantity = 1,
                    Item = item
                };
                cartHead.Items.Add(cartrow);
            }
            else
                cartrow.Quantity += 1;
            context.SaveChanges();
            return ret;
        }

    }
}
