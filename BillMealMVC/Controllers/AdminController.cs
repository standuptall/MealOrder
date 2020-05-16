using BillMealMVC.Model;
using BillMealMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class AdminController :  BaseController
    {
        MealContext context = new MealContext();
        // GET: Admin
        public AdminController():base()
        {
        }
        [Authorize]
        public ActionResult Index()
        {
            return Redirect("/admin/products");
        }
        
        [Authorize]
        public ActionResult Products()
        {
            var categories = context.ItemCategories.ToList();
            ViewBag.Categories = categories;
            var items = context.Items.ToList();
            return View(items);
        }
        [Authorize]
        public ActionResult Orders()
        {
            var closed = context.Cart.Where(c=>c.Closed).OrderByDescending(c=>c.CreationDate).ToList();
            var open = context.Cart.Where(c => !c.Closed).OrderByDescending(c => c.CreationDate).ToList();
            ViewBag.Closed = closed;
            ViewBag.Open = open;
            return View();
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            Item Item = null;
            if (id!=null && id>0)
            {
                Item = context.Items.Where(c => c.ItemId == id).FirstOrDefault();
                ViewBag.currentItem = Item;
            }
            if (Item==null)
                Item = new Item();
            var categories = context.ItemCategories.ToList();
            ViewBag.Categories = categories;
            return View(Item);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit()
        {
            var id = int.Parse(HttpContext.Request.Form["ItemId"]);
            var _currentItem = context.Items.Where(c => c.ItemId == id).FirstOrDefault();
            if (_currentItem == null)
            {
                _currentItem = new Item();
            }
            _currentItem.Name = HttpContext.Request.Form["Name"];
            _currentItem.Description = HttpContext.Request.Form["Description"];
            _currentItem.CategoryId = int.Parse(HttpContext.Request.Form["CategoryId"]);
            _currentItem.Price = double.Parse(HttpContext.Request.Form["Price"],CultureInfo.InvariantCulture);
            _currentItem.DiscountAmount = double.Parse(HttpContext.Request.Form["DiscountAmount"], CultureInfo.InvariantCulture);
            _currentItem.DiscountPercent = double.Parse(HttpContext.Request.Form["DiscountPercent"], CultureInfo.InvariantCulture);
            
            if (_currentItem.ItemId == default(int))
                context.Items.Add(_currentItem);
            context.SaveChanges();
            if (Request.Files.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    var newfile =Path.Combine(Server.MapPath("~"), "imgs", _currentItem.ItemId + ".jpg");
                    Request.Files[0].SaveAs(newfile);
                }
            }
            return Redirect("/Admin");
        }
         [Authorize]
         public ActionResult ViewOrder(int id)
        {
            var order = context.Cart.Where(c => c.CartId == id)
                .Include(c=>c.Items)
                .FirstOrDefault();
            return View(order);
        }
        [Authorize]
        public JsonResult SendEmail(int id)
        {
            var order = context.Cart.Where(c => c.CartId == id)
                .Include(c => c.Items)
                .FirstOrDefault();
            var template = @"
<div>
<h2>Il tuo ordine è stato confermato.</h2>
<li>";
            foreach (var item in order.Items)
            {
                template += "<ul>" + item.ItemName + "</ul>";
            }
            template += @"
</li>
<p>Totale: " + Utils.FormatPrice(order.ConfirmedTotal) + @"</p>
</div>
";
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(order.Email);
                mailMessage.From = new MailAddress("noreply@italianburgerpavia.it");
                mailMessage.Sender = new MailAddress(_settings.UserEmail);
                mailMessage.Subject = "Il tuo ordine è stato confermato";
                mailMessage.Body = template;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient("pro.eu.turbo-smtp.com");


                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Host = "pro.eu.turbo-smtp.com";
                smtpClient.Port = 25;
                smtpClient.Credentials = new NetworkCredential(_settings.UserEmail, _settings.PasswordEmail);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("ops");
            }
            order.EmailSent = true;
            context.SaveChanges();
            return Json(new { message = "ok" }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult Cancel(int id)
        {
            var order = context.Cart.Where(c => c.CartId == id)
                .Include(c => c.Items)
                .FirstOrDefault();
            
            order.Canceled = true;
            context.SaveChanges();
            return Json(new { message = "ok" }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public ActionResult Categories()
        {
            var cats = context.ItemCategories.Select(c => new ItemCategoryModel
            {
                CategoryId = c.CategoryId,
                Description = c.Description
            }).ToList();
            foreach(var cat in cats)
            {
                cat.NumberOfProducts = context.Items.Where(c => c.CategoryId == cat.CategoryId).Count();
            }
            return View("Categories",cats);
        }
        [Authorize]
        public ActionResult ViewCategory(int? id)
        {
            if (Request.HttpMethod.ToLower() == "post")
            {
                return SaveCategory();
            }
            ItemCategoryModel cat = null;
            if (id != null)
            {
                cat = context.ItemCategories.Where(c => c.CategoryId == id).Select(c => new ItemCategoryModel
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description
                }).FirstOrDefault();
                cat.NumberOfProducts = context.Items.Where(c => c.CategoryId == id).Count();
            }
            if (cat == null)
                cat = new ItemCategoryModel();
            return View(cat);
        }
        [Authorize]
        public ActionResult SaveCategory()
        {
            var Description = HttpContext.Request.Form["Description"];
            var idstring = HttpContext.Request.Form["CategoryId"];
            int id = 0;
            if (!string.IsNullOrEmpty(idstring))
                id = int.Parse(idstring);
            ItemCategory cat = null;
            if (id > 0){
                cat = context.ItemCategories.Where(c => c.CategoryId == id).FirstOrDefault();
                if (cat == null)
                    return Categories();
                cat.Description = Description;
            }else
            {
                cat = new ItemCategory();
                cat.Description = Description;
                context.ItemCategories.Add(cat);
            }
            context.SaveChanges();
            return Categories();
        }
        [Authorize]
        public ActionResult DeleteCategory(int id)
        {
            var cat = context.ItemCategories.Where(c => c.CategoryId == id).FirstOrDefault();
            if (cat == null)
                return Categories();
            context.ItemCategories.Remove(cat);
            context.Entry<ItemCategory>(cat).State = EntityState.Deleted;
            context.SaveChanges();
            return Categories();
        }
    }
}
