using BillMealMVC.Model;
using BillMealMVC.Models;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Services.Description;
using System.Web.UI.HtmlControls;

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
            var cart = Utils.GetCart(context, request);
            double number = 0;
            if (cart != null)
                number = cart.Items.Select(c => c.Quantity).Sum();
            var divprice = new TagBuilder("span");
            divprice.AddCssClass("badge badge-light");
            divprice.InnerHtml = ((int)number).ToString();
            var html = divprice.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html);
        }
        public static IHtmlString CartPreview(this HtmlHelper htmlHelper, HttpRequestBase request)
        {
            //    < div class="btn-group">
            //    <form class="p-4">
            //        <div class="dropdown-menu dropdown-menu-right p-4">
            //            <div class="row">
            //                <div class="col-md-6">Panino e panelle</div>
            //                <div class="col-md-6"><input type = "number" class="form-control" value="2" /></div>
            //            </div>
            //            <div class="dropdown-divider"></div>
            //            <div class="row">
            //                <div class="col-md-6">Coca cola</div>
            //                <div class="col-md-6"><input type = "number" class="form-control" value="2" onchange="cart_reload(1,this)" /></div>
            //            </div>
            //            <div class="dropdown-divider"></div>
            //        </div>
            //    </form>
            //</div>
            var context = new MealContext();
            var cart = Utils.GetCart(context, request);
            var bd = new StringBuilder();
            var divext = new TagBuilder("div");
            divext.AddCssClass("btn-group");
            var form = new TagBuilder("div");
            form.Attributes.Add("style", "font-size:smaller!important;");
            form.AddCssClass("p-2");
            var divint = new TagBuilder("div");
            divint.AddCssClass("dropdown-menu dropdown-menu-right p-4");
            bd.Append(divext.ToString(TagRenderMode.StartTag));
            bd.Append(form.ToString(TagRenderMode.StartTag));
            bd.Append(divint.ToString(TagRenderMode.StartTag));
            if (cart == null || cart?.Items.Count == 0)
                bd.Append("<div class=\"row\"><small>Nessun prodotto</small></div>");
            else
            {
                var tb = "<table class=\"table table-borderless\">";
                tb += "<thead>";
                tb += "<tr>";
                tb += "<th scope=\"col\">Prodotto</th>";
                tb += "<th scope=\"col\">Quantità</th>";
                tb += "</tr></thead><tbody>";

                foreach (var row in cart.Items)
                {
                    tb += "<tr>";
                    tb += "<td>"+row.ItemName+"</td>";
                    tb += "<td>" + row.Quantity.ToString(CultureInfo.InvariantCulture) + "</td>";
                    tb += "</tr>";
                    //var divrow = new TagBuilder("div");
                    //divrow.AddCssClass("row");
                    //var col1 = new TagBuilder("div");
                    //col1.AddCssClass("col-md-6");
                    //col1.InnerHtml = "<small>"+row.ItemName+"</small>";
                    //var col2 = new TagBuilder("div");
                    //col2.AddCssClass("col-md-6");
                    //var input = new TagBuilder("input");
                    //input.AddCssClass("");
                    //input.Attributes.Add("type", "number");
                    //input.Attributes.Add("value", row.Quantity.ToString(CultureInfo.InvariantCulture));
                    //input.Attributes.Add("onchange", "cart_reload(" + row.CartRowId + ", this)");
                    //col2.InnerHtml = input.ToString(TagRenderMode.SelfClosing);
                    //bd.Append(divrow.ToString(TagRenderMode.StartTag));
                    //bd.Append(col1.ToString(TagRenderMode.Normal));
                    //bd.Append(col2.ToString(TagRenderMode.Normal));
                    //bd.Append(divrow.ToString(TagRenderMode.EndTag));
                }
                tb += "</tbody></table>";
                bd.Append(tb);
            }
            bd.Append(divext.ToString(TagRenderMode.EndTag));
            bd.Append(form.ToString(TagRenderMode.EndTag));
            bd.Append(divint.ToString(TagRenderMode.EndTag));
            return MvcHtmlString.Create(bd.ToString());
        }
        public static IHtmlString PrintItemsTable(this HtmlHelper htmlHelper, CartHead cart)
        {
            //        < table class="table">
            //            <thead class="thead-dark">
            //                <tr>
            //                    <th>Prodotto</th>
            //                    <th>Quantità</th>
            //                    <th>Prezzo</th>
            //                </tr>
            //            </thead>
            //            @{
            //                var totale = 0.0;
            //}
            //            <tbody>
            //                @foreach(BillMealMVC.Models.CartRows row in Model.Items)
            //{
            //                    < tr >
            //                        < td >< img class="cart-thumbnail" src="~/imgs/@(row.ItemId).jpg" />@row.ItemName</td>
            //                        <td><input class="form-control" style="width:100px;" value="@row.Quantity" type="number" name="Quantity" onchange="cart_reload(@(row.CartRowId),this)" /></td>
            //                        <td>@Utils.FormatPrice(row.ItemPrice)</td>
            //                    </tr>
            //                    totale += (row.ItemPrice* row.Quantity);
            //                }
            //            </tbody>
            //            <tfoot>
            //                <tr>
            //                    <td></td>
            //                    <td></td>
            //                    <th>Totale: @Utils.FormatPrice(totale)</th>
            //                </tr>
            //            </tfoot>
            //        </table>
            var table = new TagBuilder("table");
            table.AddCssClass("table");
            var thead = new TagBuilder("thead");
            thead.AddCssClass("thead-dark");
            var rowhead = new TagBuilder("tr");
            throw new NotImplementedException();
        }
    }
}