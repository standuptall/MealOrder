using BillMealMVC.Model;
using BillMealMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            var cart = Utils.GetCart(context,Request,Response);
            
            if (cart == null || cart.Items.Count == 0)
                return Redirect("/Products");
            LoadDeliveries();
            return View("Index",cart);
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
            var cart = Utils.GetCart(context, Request,Response);
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

            var cart = Utils.GetCart(context, Request,Response);
            var Notes = HttpContext.Request.Form["Notes"];
            var Email = HttpContext.Request.Form["Email"];
            var Phone = HttpContext.Request.Form["Phone"];
            var delivery = HttpContext.Request.Form["deliverytime"];
            var paymentmethod = HttpContext.Request.Form["paymentmethod"];
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
                cart.ConfirmedTotal = cart.Items.Select(c => c.ItemPrice*c.Quantity).Sum();
                context.SaveChanges();
                if (paymentmethod == "paypal")
                {
                    return View("Payment", cart);
                }
                else
                {
                    return Success(cart);
                }
            }

            return Redirect("Products");
        }
        public ActionResult Success(CartHead cart)
        {
            Response.Cookies["meal_id"].Expires = DateTime.Now.AddDays(-1);
            cart.Closed = true;
            cart.ClosedDate = DateTime.Now;
            context.SaveChanges();
            return View("Success");
        }
        public ActionResult gabhv5255sdna4dv4tac52n4s2bz5c256f5x6v2n5s6a56f59asd5g2s6bs6()
        {
            LogRequest(Request);
            var verificationRequest = (HttpWebRequest)WebRequest.Create("https://ipnpb.paypal.com/cgi-bin/webscr");

            //Set values for the verification request
            verificationRequest.Method = "POST";
            verificationRequest.ContentType = "application/x-www-form-urlencoded";
            var param = Request.BinaryRead(Request.ContentLength);
            var strRequest = Encoding.ASCII.GetString(param);

            //Add cmd=_notify-validate to the payload
            strRequest = "cmd=_notify-validate&" + strRequest;
            verificationRequest.ContentLength = strRequest.Length;

            //Attach payload to the verification request
            var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            var dict = new Dictionary<string, string>();
            dict.Add("Payment_status", Request.Form["Payment_status"]);
            dict.Add("Txn_id", Request.Form["Txn_id"]);
            dict.Add("Receiver_email", Request.Form["Receiver_email"]);
            dict.Add("mc_gross", Request.Form["mc_gross"]);
            dict.Add("invoice", Request.Form["invoice"]);
            //Fire and forget verification task
            Task.Run(() => VerifyTask(verificationRequest,dict));

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void VerifyTask(HttpWebRequest verificationRequest, Dictionary<string, string> dict)
        {
            var verificationResponse = string.Empty;
            WebResponse response = null;
            try
            {
                
                //Send the request to PayPal and get the response
                response = verificationRequest.GetResponse();
                var streamIn = new StreamReader(response.GetResponseStream());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();

            }
            catch (Exception ex)
            {

                context.RequestLog.Add(new RequestLog
                {
                    Date = DateTime.Now,
                    Key = "VerifyTask error" + " " + ex.StackTrace,
                    Value = ex.Message
                });
                context.SaveChanges();
            }

            ProcessVerificationResponse(verificationResponse,dict);

        }

        private void ProcessVerificationResponse(string verificationResponse, Dictionary<string, string> dict)
        {
            var invoice = dict["invoice"];
            try
            {
                if (verificationResponse.Equals("VERIFIED"))
                {
                    var Payment_status = dict["Payment_status"];
                    var Txn_id = dict["Txn_id"];
                    var Receiver_email = dict["Receiver_email"];
                    var Payment_amount = dict["mc_gross"];
                    var idcart = int.Parse(invoice.Substring(invoice.IndexOf("#") + 1, invoice.Length - invoice.IndexOf("#") - 1));
                    var cart = context.Cart.Where(c => c.CartId == idcart).FirstOrDefault();
                    if (cart == null)
                        throw new Exception("Ordine " + idcart + " non trovato!");
                    if (cart.Closed)
                        throw new Exception("Ordine " + idcart + " era già chiuso!");
                    if (Payment_status != "Completed")
                        throw new Exception("Payload ricevuto tuttavia il pagamento non è completo");
                    cart.TotalPaid = double.Parse(Payment_amount, CultureInfo.InvariantCulture);
                    cart.TransactionId = Txn_id;
                    cart.Closed = true;
                    cart.ClosedDate = DateTime.Now;
                    context.SaveChanges();
                }
                else if (verificationResponse.Equals("INVALID"))
                {

                    context.RequestLog.Add(new RequestLog
                    {
                        Date = DateTime.Now,
                        Key = "INVALID",
                        Value = "INVALID "+ invoice
                    });
                    context.SaveChanges();
                }
                else
                {

                    context.RequestLog.Add(new RequestLog
                    {
                        Date = DateTime.Now,
                        Key = "verificationResponse string not valid " + invoice,
                        Value = verificationResponse
                    });
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                context.RequestLog.Add(new RequestLog
                {
                    Date = DateTime.Now, 
                    Key = "exception during verificationResponse " + invoice+ " "+ex.StackTrace,
                    Value = ex.Message
                });
                context.SaveChanges();
            }
        }

        private void LogRequest(HttpRequestBase request)
        {
            context.RequestLog.Add(new RequestLog
            {
                Date = DateTime.Now,
                Key = "Chiamata",
                Value = "Chiamata da IP " + request.UserHostAddress
            });
            context.SaveChanges();
        }

        public ActionResult Payment(int? orderid)
        {
            if (orderid == null)
                return Redirect("/Products");
            var cart = context.Cart.Where(c => c.CartId == orderid.Value).FirstOrDefault();
            if (cart == null)
                return Redirect("/products");
            return Success(cart);
        }
        public ActionResult PaymentCanceled(int? orderid)
        {
            return Index();
        }
    }
}