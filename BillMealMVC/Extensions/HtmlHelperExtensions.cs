using BillMealMVC.Model;
using BillMealMVC.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BillMealMVC.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString Price(this HtmlHelper htmlHelper, Item item)
        {/*
            <div class="price-frame list-inline">
                                <div class="price price-strike list-inline-item">10€</div>
                                <div class="discount list-inline-item">10%</div>
                                <div class="price list-inline-item">7.99€</div>
                            </div>
                            
             */
            var newprice = BillMealMVC.Utils.GetPrice(item);
            var divframe = new TagBuilder("div");
            divframe.AddCssClass("price-frame list-inline");
            divframe.InnerHtml = item.Price.ToString(CultureInfo.CurrentCulture) + "€";
            var divprice = new TagBuilder("div");
            divprice.AddCssClass("price list-inline-item");
            divprice.InnerHtml = item.Price.ToString(CultureInfo.CurrentCulture)+"€";
            var divdiscount = new TagBuilder("div");
            divdiscount.AddCssClass("discount list-inline-item");
            divdiscount.InnerHtml = item.DiscountPercent.ToString(CultureInfo.CurrentCulture) + "%";
            var divnewprice = new TagBuilder("div");
            divnewprice.AddCssClass("price list-inline-item");
            divnewprice.InnerHtml = newprice.ToString(CultureInfo.CurrentCulture) + "€";


            StringBuilder htmlBuilder = new StringBuilder();
            if (newprice < item.Price)
            {
                divprice.AddCssClass("price-strike");
                htmlBuilder.Append(divprice.ToString(TagRenderMode.Normal));
            }
            if (item.DiscountPercent>0)
                htmlBuilder.Append(divdiscount.ToString(TagRenderMode.Normal));
            htmlBuilder.Append(divnewprice.ToString(TagRenderMode.Normal));
            divframe.InnerHtml = htmlBuilder.ToString();
            var html = divframe.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html);
        }
        public static IHtmlString CartNumberOfItems(this HtmlHelper htmlHelper, HttpRequestBase request)
        {
            var context = new MealContext();
            var cook = request.Cookies.Get("meal_id");
            double number = 0;
            if (cook != null)
            {
                var cart = context.Cart.Where(c => c.meal_id == cook.Value)
                    .Include(c=>c.Items)
                    .FirstOrDefault();
                if (cart != null)
                    number = cart.Items.Select(c => c.Quantity).Sum();
            }
            var divprice = new TagBuilder("span");
            divprice.AddCssClass("badge badge-light");
            divprice.InnerHtml = ((int)number).ToString();
            var html = divprice.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html);
        }
    }
}