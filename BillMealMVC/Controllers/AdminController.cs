using BillMealMVC.Model;
using BillMealMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class AdminController : Controller
    {
        MealContext context = new MealContext();
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            var categories = context.ItemCategories.ToList();
            ViewBag.Categories = categories;
            var items = context.Items.ToList();
            return View(items);
        }
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
                    var newfile = Path.Combine(ConfigurationManager.AppSettings["imagepath"].ToString(),"imgs", _currentItem.ItemId + ".jpg");
                    Request.Files[0].SaveAs(newfile);
                }
            }
            return Redirect("/Admin");
        }
    }
}
