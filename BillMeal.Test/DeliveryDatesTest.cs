using BillMealMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BillMeal.Test
{
    [TestClass]
    public class DeliveryDatesTest
    {
        [TestMethod]
        public void TestDeliveryDates()
        {
            var controller = new CheckoutController();
            controller.Request.Cookies.Add(new System.Web.HttpCookie("meal_id", "a7d1c44f-39e2-430e-b5bd-e614be20da7f"));
            var result = controller.Index() as ViewResult;
            var deliverydates = (List<string[]>)result.ViewBag.Deliveries;
            Assert.IsTrue(deliverydates.Count > 0);

            
        }
    }
}
