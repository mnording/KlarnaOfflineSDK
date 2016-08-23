using Klarna.Offline.Entities;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Klarna.Offline
{
    public class OfflineOrder : Klarna.Entities.Order
    {
        string merchantReference;
        string phone;
        public enum Status { NotSent,Sent,Pending,Polling,Complete}
        Status status;
        MerchantConfig config;
        Klarna.Entities.Cart cart;
        Uri postbackUri;
        string klarnaId;
        string statusUrl;
        string terminalId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        public OfflineOrder(Klarna.Entities.Cart cart, Entities.MerchantConfig config, string terminal, string phonenumber) : base(cart, config)
        {
            Regex r = new Regex(@"^([+]46)\s*(7[0236])\s*(\d{4})\s*(\d{3})$", RegexOptions.IgnoreCase);
            if(!r.IsMatch(phonenumber))
            {
                throw new ArgumentException("Phone number incorrect");
            }
            this.phone = phonenumber;
            this.status = Status.NotSent;
            this.cart = cart;
            this.config = config;
            terminalId = terminal;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        /// <param name="merchantReference">The store-reference for this order</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference) : this(cart, config,terminal, phonenumber)
        {
            this.merchantReference = merchantReference;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        /// <param name="merchantReference">The store-reference for this order</param>
        /// <param name="postbackUri">The URL on your end that Klanra will push order data to after completion.</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference,Uri postbackUri) : this(cart, config, terminal, phonenumber)
        {
            this.merchantReference = merchantReference;
            this.postbackUri = postbackUri;
          
        }
        /// <summary>
        /// Activating the order. Effectively sending the SMS to the customer phone
        /// </summary>
        public void Create()
        {
            sendOrder();
        }
        private void sendOrder()
        {
            WebRequest request = WebRequest.Create("https://buy.playground.klarna.com/v1/"+config.MerchantId+"/orders");
            request.Method = "POST";
            JsonSerializer _jsonWriter = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            JObject ob = JObject.FromObject(this, _jsonWriter);
           // JObject jObject = JObject.Parse("{  \n   \"mobile_no\":\"+46700012794\",\n   \"merchant_reference1\":\"1\",\n   \"purchase_currency\":\"sek\",\n   \"purchase_country\":\"se\",\n   \"locale\":\"sv-se\",\n   \"postback_uri\":\"http://requestbin.herokuapp.com/tbh5v5tb\",\n   \"order_lines\":[  \n      {  \n         \"unit_price\":10000,\n         \"quantity\":1,\n         \"reference\":\"string\",\n         \"tax_rate\":2500,\n         \"name\":\"string\"\n      }\n   ]\n}");
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(config.MerchantId, config.SharedSecret);
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + digest);
            var test = ob.ToString();
          
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(ob.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }
            WebResponse response = request.GetResponse();
            status = Status.Sent;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd(); // do something fun...
                JObject orderResponse = JObject.Parse(result);
                klarnaId = orderResponse["id"].ToString();
                status = Status.Pending;
                if(orderResponse["status_uri"] != null)
                {
                    statusUrl = orderResponse["status_uri"].ToString();
                    status = Status.Polling;
                }
            }
           
        }
        /// <summary>
        /// Will fetch data from klarna endpoint to find customer information
        /// </summary>
        /// <param name="url">Url to poll</param>
        /// <returns>OrderDtails Object if purchase is complete</returns>
        public OrderDetails pollData(string url)
        {
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(config.MerchantId, config.SharedSecret);
            WebRequest request = WebRequest.Create(statusUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + digest);
            try
            {
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd(); // do something fun...
                    Helpers.JsonConverter.GetOrderFromString(result);
                    var jsreader = new JsonTextReader(new StringReader(result));
                    var details=  new JsonSerializer().Deserialize<OrderDetails>(jsreader);
                    return details;
                   
                }
            }
            catch(Exception e)
            {
                return null;
            }
           
           
        }
        
        public string mobile_no
        {
            get { return phone; }
        }
        public string merchant_reference1
        {
            get { return this.merchantReference; }
        }
        public string purchase_currency
        {
            get { return config.Currency.ToLower(); }
        }
        public string postback_uri
        {
            get
            {
                if (postbackUri != null)
                {
                    return postbackUri.AbsoluteUri;
                }
                return null;
            }
        }
        public string purchase_country
        {
            get { return config.Country.ToLower(); }
        }
        public string locale
        {
            get { return config.Locale.ToLower(); }
        }
        public List<Klarna.Entities.CartRow> order_lines
        {
            get { return cart.OrderLines; }
        }
        public Status GetStatus()
        {
            return status;
        }
        public string GetStatusUrl() { return statusUrl; }
    }
}
