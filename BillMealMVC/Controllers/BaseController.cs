using BillMealMVC.Model;
using BillMealMVC.Models;
using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class BaseController : Controller
    {
        protected MealContext _context = new MealContext();
        protected Settings _settings;
        public BaseController()
        {
            _settings = _context.Settings.FirstOrDefault();
            var cats = _context.ItemCategories.ToList();
            ViewBag.Categories = cats;
            ViewBag.FooterDescription = _settings.FooterDescription;
            ViewBag.AppName = _settings.AppName;
        }
    }
}