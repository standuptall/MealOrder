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
            var cart = Utils.GetCart(context,Request);
            if (cart.Items.Count == 0)
                return Redirect("/Products");
            var ora_apertura = 7;
            var ora_chiusura = 21;
            var ora_now = DateTime.Now.Hour;
            var collection = new List<string[]>();
            var start = (ora_now > ora_apertura ? ora_now : ora_apertura);
            for(double i = start; i < ora_chiusura; i += 0.25)
            {
                var ore = Math.Floor(i);
                var minuti = (i - ore) * 60;
                collection.Add(new string[] { ore.ToString("00") + minuti.ToString("00"),ore.ToString("00")+":"+minuti.ToString("00") });
            }
            ViewBag.Deliveries = collection;
            return View(cart);
        }
        public ActionResult review(int id,int qty)
        {
            var cart = Utils.GetCart(context, Request);
            if (cart != null) { 
                var item = cart.Items.Where(c => c.CartRowId == id).FirstOrDefault();
                if (item != null)
                {
                    if (qty <= 0)
                    {
                        cart.Items.Remove(item);
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    else
                    {
                        item.Quantity = qty;
                    }
                    context.SaveChanges();
                }
            }

            return View("Index",cart);
        }
        [HttpPost]
        public ActionResult OnPost()
        {
            var cart = Utils.GetCart(context, Request);
            var Notes = HttpContext.Request.Form["Notes"];
            var Email = HttpContext.Request.Form["Email"];
            var Phone = HttpContext.Request.Form["Phone"];
            var delivery = HttpContext.Request.Form["deliverytime"];
            var DeliveryHour = int.Parse(delivery.Substring(0, 2));
            var DeliveryMinute = int.Parse(delivery.Substring(2, 2));

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