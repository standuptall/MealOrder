using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using BillMealMVC.Controllers;
using BillMealMVC.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace BillMeal.Test
{
    [TestClass]
    public class ProductsControllerTest
    {
        protected HttpClient _client;
        [TestInitialize]
        public void Setup()
        {
        }
        [TestMethod]
        public void TestProductsIndex()
        {
            var controller = new ProductsController();
            var result = controller.Index(1) as ViewResult;
            Assert.AreEqual(typeof(List<Item>), result.Model.GetType());
            result = controller.Index(2) as ViewResult;
            Assert.AreEqual(typeof(List<Item>), result.Model.GetType());
            result = controller.Index(3) as ViewResult;
            Assert.AreEqual(typeof(List<Item>), result.Model.GetType());

        }
        [TestMethod]
        public void TestAddToCart()
        {
            //var controller = new ProductsController();
            //var result = controller.Index(null) as ViewResult;
            //var product = ((List<Item>)result.Model)[0];
            //var datenow = DateTime.Now;
            //var resultcart = controller.AddToCart(product.ItemId);
            ////controllo che è stato creato un cart
            //var controllerOrder = new AdminController();
            //var orders = controllerOrder.Orders() as ViewResult;
            //var orderopen = ((List<CartHead>)(orders.ViewBag.Open)).Where(c => c.CreationDate > datenow)
            //                                                        .FirstOrDefault() ;
            //Assert.IsNotNull(orderopen);
            ////controllo che contenga il prodotto di prima
            //Assert.AreEqual(orderopen.Items.ToList()[0].ItemId , product.ItemId);
        }
    }
}
