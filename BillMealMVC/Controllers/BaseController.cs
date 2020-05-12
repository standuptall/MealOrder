using BillMealMVC.Model;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class BaseController : Controller
    {
        protected MealContext _context = new MealContext();
        public BaseController()
        {
            ViewBag.AppName = "Italian Burger Pavia";
        }
    }
}