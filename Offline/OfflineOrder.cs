using Klarna.Offline.Entities;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Klarna.Offline
{
    public class OfflineOrder : Klarna.Entities.Order
    {
        string _merchantReference;

        public enum Status { NotSent,Sent,Pending,Polling,Complete}
        Status _status;
        MerchantConfig _config;
        Klarna.Entities.Cart _cart;
        Uri _postbackUri;
        string _klarnaId;
        Uri _statusUrl;
        public string sms_sender_id;
        public string sms_text;
        string _terminalId;
        public bool auto_activate;
        private void verifyPhoneForCountry()
        {
            Regex r = new Regex("");
            if (_config.Country == "SE")
            {
                r = new Regex(@"^([+]46)\s*(7[0236])\s*(\d{4})\s*(\d{3})$", RegexOptions.IgnoreCase);
            }
            else if(_config.Country == "FI")
            {
                r = new Regex(@"^((90[0-9]{3})?0|\+358\s?)(?!(100|20(0|2(0|[2-3])|9[8-9])|300|600|700|708|75(00[0-3]|(1|2)\d{2}|30[0-2]|32[0-2]|75[0-2]|98[0-2])))(4|50|10[1-9]|20(1|2(1|[4-9])|[3-9])|29|30[1-9]|71|73|75(00[3-9]|30[3-9]|32[3-9]|53[3-9]|83[3-9])|2|3|5|6|8|9|1[3-9])\s?(\d\s?){4,19}\d$", RegexOptions.IgnoreCase);
            }
            else if (_config.Country == "NO")
            { 
                 r = new Regex(@"^(0047|\+47|47)?[2-9]\d{7}$", RegexOptions.IgnoreCase);
            }
            else{
                r = new Regex("xxx");
            }

            if (!r.IsMatch(mobile_no))
            {
                throw new ArgumentException("Phone number incorrect");
            }
        }
        /// <summary>
        /// Initiating a new Klarna Offline Order 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        /// <param name="merchantReference">The store-reference for this order</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference, bool autoActivate=true) : base(cart, config)
        {
            mobile_no = phonenumber;
            _status = Status.NotSent;
            _cart = cart;
            _config = config;
            _terminalId = terminal;
            auto_activate = autoActivate;
            verifyPhoneForCountry();
            _merchantReference = merchantReference;
           
        }
        /// <summary>
        /// Initiating a new Klarna Offline Order 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        /// <param name="merchantReference">The store-reference for this order</param>
        /// <param name="postbackUri">The URL on your end that Klanra will push order data to after completion.</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference,Uri postbackUri, bool autoActivate = true) : this(cart, config, terminal, phonenumber, merchantReference, autoActivate)
        {
            _postbackUri = postbackUri;
          
        }
        /// <summary>
        /// Activating the order. Effectively sending the SMS to the customer phone
        /// </summary>
        public void Create()
        {
            SendOrder();
        }
        private void SendOrder()
        {
            WebRequest request = WebRequest.Create(GetBaseUrl() + "/v1/" + _config.MerchantId+"/orders");
            request.Method = "POST";
            JsonSerializer jsonWriter = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            JObject ob = JObject.FromObject(this, jsonWriter);
          
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(_config.MerchantId, _config.SharedSecret);
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + digest);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(ob.ToString());
            }
            WebResponse response = request.GetResponse();
            _status = Status.Sent;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd(); // do something fun...
                JObject orderResponse = JObject.Parse(result);
                _klarnaId = orderResponse["id"].ToString();
                _status = Status.Pending;
                if(orderResponse["status_uri"] != null)
                {
                    _statusUrl = new Uri(orderResponse["status_uri"].ToString());
                    _status = Status.Polling;
                }
            }
           
        }
        /// <summary>
        /// Will fetch data from Klarna endpoint to find customer information
        /// </summary>
        /// <param name="url">Url to poll</param>
        /// <returns>OrderDtails Object if purchase is complete</returns>
        public OrderDetails pollData(Uri url)
        {
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(_config.MerchantId, _config.SharedSecret);
            WebRequest request = WebRequest.Create(_statusUrl);
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
            catch(Exception)
            {
                return null;
            }
           
           
        }
        /// <summary>
        /// Cancels the order
        /// </summary>
        public void Cancel()
        {
            if(_klarnaId == String.Empty)
            {
                throw new Exception("Cannot cancel an order that has not been created");
            }
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(_config.MerchantId, _config.SharedSecret);
            WebRequest request = WebRequest.Create(GetBaseUrl()+"/v1/" + _config.MerchantId + "/orders/"+_klarnaId+"/cancel");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + digest);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Something went wrong with the cancellation");
            }
        }
        public void SetTextMessage(string text)
        {
           if(text.IndexOf("{URL}",StringComparison.OrdinalIgnoreCase)>=0)
            {
                var index = text.IndexOf("{URL}", StringComparison.OrdinalIgnoreCase);
                var firstpart = text.Substring(0, index);
                var secondpart = text.Substring(index+5);
                text = firstpart+" {url} "+secondpart;
            }
            if(text.Contains("{url}") == false)
            {
                text = text + " {url}";
            }
            sms_text = text;
        }
        public void SetSender(string sender)
        {
            sms_sender_id = sender;
        }
        public string mobile_no { get; }

        public string merchant_reference1
        {
            get { return _merchantReference; }
        }
        public string purchase_currency => _config.Currency.ToLower();

        public string postback_uri
        {
            get
            {
                return _postbackUri != null ? _postbackUri.AbsoluteUri : null;
            }
        }
        public string purchase_country
        {
            get { return _config.Country.ToLower(); }
        }
        public string locale
        {
            get { return _config.Locale.ToLower(); }
        }
        public List<Klarna.Entities.CartRow> order_lines
        {
            get { return _cart.OrderLines; }
        }
        public Status GetStatus()
        {
            return _status;
        }
        public Uri GetStatusUrl() { return _statusUrl; }
        private string GetBaseUrl()
        {
            if(_config.Enviournment == MerchantConfig.Server.Live)
            {
                return "https://buy.klarna.com";
            }
            return "https://buy.playground.klarna.com";
        }
    }
}
