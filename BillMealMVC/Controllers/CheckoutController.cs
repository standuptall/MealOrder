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
    public class CheckoutController  : BaseController
    {
        MealContext context = new MealContext(); 
        public CheckoutController() : base()
        {

        }
        public ActionResult Index()
        {
            if (Request.HttpMethod.ToLower()== "post")
            {
                return Confirm();
            }
            var cart = Utils.GetCart(context,Request);
            
            if (cart == null || cart.Items.Count == 0)
                return Redirect("/Products");
            LoadDeliveries();
            return View(cart);
        }
        private void LoadDeliveries()
        {

            var ora_apertura = 7;
            var ora_chiusura = 21;
            var ora_now = DateTime.Now.Hour;
            var collection = new List<string[]>();
            double start = 0;
            if (ora_now > ora_apertura && ora_now < ora_chiusura)
                start = ora_now;
            else if (ora_now >= ora_chiusura)
                start = ora_apertura;
            else if (ora_now < ora_apertura)
                start = ora_apertura;
            //start += Math.Ceiling((double)DateTime.Now.Minute / 60);
            for (double i = start; i < ora_chiusura; i += 0.25)
            {
                var ore = Math.Floor(i);
                var minuti = (i - ore) * 60;
                collection.Add(new string[] { ore.ToString("00") + minuti.ToString("00"), ore.ToString("00") + ":" + minuti.ToString("00") });
            }
            ViewBag.Deliveries = collection;
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
            LoadDeliveries();
            return View("Index",cart);
        }
        [HttpPost]
        public ActionResult Confirm()
        {
            ViewBag.ErrorMessage = null;

            var cart = Utils.GetCart(context, Request);
            var Notes = HttpContext.Request.Form["Notes"];
            var Email = HttpContext.Request.Form["Email"];
            var Phone = HttpContext.Request.Form["Phone"];
            var delivery = HttpContext.Request.Form["deliverytime"];
            int DeliveryHour, DeliveryMinute;
            LoadDeliveries();
            try
            {
                DeliveryHour = int.Parse(delivery.Substring(0, 2));
                DeliveryMinute = int.Parse(delivery.Substring(2, 2));
            }
            catch 
            {
                ViewBag.ErrorMessage = "Si è verificato un errore durante la conferma. Prego riprovare";
                return View("Index",cart);
            }
            var now = DateTime.Now;
            var tomorrow = DateTime.Now.AddDays(1);
            var usedate = DeliveryHour < now.Hour ? tomorrow : now;
            var deliverydate = new DateTime(usedate.Year, usedate.Month, usedate.Day, DeliveryHour, DeliveryMinute, 0);
            if (string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(Phone)
                    )
            {
                ViewBag.ErrorMessage = "Campi non compilati o ora consegna non valida!";
                return View("Index", cart);
            }

            if (cart != null)
            {
                cart.Notes = Notes;
                cart.Email = Email;
                cart.Phone = Phone;
                cart.DeliveryDate = deliverydate;
                cart.Closed = true;
                cart.ClosedDate = DateTime.Now;
                cart.ConfirmedTotal = cart.Items.Select(c => c.ItemPrice*c.Quantity).Sum();
                context.SaveChanges();
                Response.Cookies["meal_id"].Expires = DateTime.Now.AddDays(-1);
                return View("Success",cart);
            }

            return Redirect("Products");
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}