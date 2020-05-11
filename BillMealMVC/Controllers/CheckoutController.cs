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
    public class CheckoutController : Controller
    {
        MealContext context = new MealContext();
        public ActionResult Index()
        {
            if (Request.HttpMethod.ToLower()== "post")
            {
                return OnPost();
            }
            CartHead cart = null;
            var meal_id = Request.Cookies.Get("meal_id");
            if (meal_id != null)
            {
                cart = context.Cart
                    .Where(c => c.meal_id == meal_id.Value)
                    .Include(c=>c.Items)
                    .FirstOrDefault();
            }
            return View(cart);
        }
        public ActionResult review(int id,int qty)
        {
            CartHead cart = null;
            var meal_id = Request.Cookies.Get("meal_id");
            if (meal_id != null)
            {
                cart = context.Cart
                    .Where(c => c.meal_id == meal_id.Value)
                    .Include(c => c.Items)
                    .FirstOrDefault();
                var item = cart.Items.Where(c => c.CartRowId == id).FirstOrDefault();
                if (item != null)
                {
                    if (qty <= 0)
                        cart.Items.Remove(item);
                    else
                        item.Quantity = qty;
                    context.SaveChanges();
                }
            }

            return View("Index",cart);
        }
        [HttpPost]
        public ActionResult OnPost()
        {
            CartHead cart = null;
            var meal_id = Request.Cookies.Get("meal_id");
            if (meal_id == null)
            {
            }
            cart = context.Cart
                .Where(c => c.meal_id == meal_id.Value)
                .Include(c => c.Items)
                .FirstOrDefault();
            var Notes = HttpContext.Request.Form["Notes"];
            var Email = HttpContext.Request.Form["Email"];
            var Phone = HttpContext.Request.Form["Phone"];
            var DeliveryHour = int.Parse(HttpContext.Request.Form["DeliveryHour"]);
            var DeliveryMinute = int.Parse(HttpContext.Request.Form["DeliveryMinute"]);

            if (cart != null)
            {
                cart.Notes = Notes;
                cart.Email = Email;
                cart.Phone = Phone;
                cart.DeliveryHour = DeliveryHour;
                cart.DeliveryMinute = DeliveryMinute;
                cart.Closed = true;
                cart.ClosedDate = DateTime.Now;
                context.SaveChanges();
                Response.Cookies.Remove("meal_id");
                return View("Success",cart);
            }

            return Redirect("Products");
        }
    }
}