using Klarna.Offline.Entities;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Klarna.Entities;
using PhoneNumbers;
using MerchantConfig = Klarna.Offline.Entities.MerchantConfig;

namespace Klarna.Offline
{
    public class OfflineOrder : Klarna.Entities.Order
    {
        string _merchantReference;

        public enum Status { NotSent, Sent, Pending, Polling, Complete }
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
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            Regex r = new Regex("");
            if (_config.Country == "SE")
            {
                var t = phoneUtil.Parse(mobile_no, "SE");
                if (phoneUtil.IsValidNumberForRegion(t, "SE"))
                {
                    return;
                }
            }
            else if (_config.Country == "FI")
            {
               var t =  phoneUtil.Parse(mobile_no, "FI");
                if (phoneUtil.IsValidNumberForRegion(t, "FI"))
                {
                    return;
                }
                
            }
            else if (_config.Country == "NO")
            {
                var t = phoneUtil.Parse(mobile_no, "NO");
                if (phoneUtil.IsValidNumberForRegion(t, "NO"))
                {
                    return;
                }
            }
            else
            {
                throw new ArgumentException("Country not supported");
            }
            throw new ArgumentException("Phone number incorrect");
        }

        /// <summary>
        /// Initiating a new Klarna Offline Order 
        /// </summary>
        /// <param name="cart">The cart for the order</param>
        /// <param name="config">The merchant config to be used</param>
        /// <param name="terminal">What terminal is the purchase from?</param>
        /// <param name="phonenumber">Phonenumber of the customer incl countrycode</param>
        /// <param name="merchantReference">The store-reference for this order</param>
        /// <param name="autoActivate">Should this order be converted to an invoice automatically?</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference, bool autoActivate = true) : base(cart, config)
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
        /// <param name="autoActivate">Should this order be converted to an invoice automatically?</param>
        public OfflineOrder(Klarna.Entities.Cart cart, MerchantConfig config, string terminal, string phonenumber, string merchantReference, Uri postbackUri, bool autoActivate = true) : this(cart, config, terminal, phonenumber, merchantReference, autoActivate)
        {
            if (postbackUri.Scheme != "https")
            {
                throw new ArgumentException("Postback URL has to be HTTPS");
            }
            _postbackUri = postbackUri;

        }

        public OfflineOrder(string orderId,MerchantConfig config):base(new Cart(), config)
        {
            _klarnaId = orderId;
            _config = config;
            _status = Status.Sent;
        }
        /// <summary>
        /// Activating the order. Effectively sending the SMS to the customer phone
        /// </summary>
        public void Create()
        {
            if (_status != Status.NotSent)
            {
                throw new Exception("Order already created. Cancel and create a new one");
               
            }
               
            SendOrder();
            _status = Status.Sent;
        }
        private void SendOrder()
        {
            JsonSerializer jsonWriter = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            JObject ob = JObject.FromObject(this, jsonWriter);
            WebResponse response = SendRequest(new Uri(GetBaseUrl() + "/v1/" + _config.MerchantId + "/orders"), "POST", ob);

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd(); // do something fun...
                JObject orderResponse = JObject.Parse(result);
                _klarnaId = orderResponse["id"].ToString();
                _status = Status.Pending;
                if (orderResponse["status_uri"] != null)
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
        /// <returns>OrderDetails Object if purchase is complete</returns>
        public OrderDetails pollData(Uri url)
        {
            _status = Status.Polling;
            try
            {
                WebResponse response = SendRequest(_statusUrl, "GET");
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd(); // do something fun...
                    Helpers.JsonConverter.GetOrderFromString(result);
                    var jsreader = new JsonTextReader(new StringReader(result));
                    var details = new JsonSerializer().Deserialize<OrderDetails>(jsreader);
                    _status = Status.Complete;
                    return details;

                }
            }
            catch (Exception)
            {
                return null;
            }


        }

        private HttpWebResponse SendRequest(Uri url, string method, JObject data = null)
        {
            var digestCreator = new Klarna.Helpers.DigestCreator();
            var digest = digestCreator.CreateOffline(_config.MerchantId, _config.SharedSecret);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/json";
            request.UserAgent = "Mnording Instore SDK - 2.3.0";
            request.Headers.Add("Authorization", "Basic " + digest);
            if (data != null)
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data.ToString());
                }
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }
        /// <summary>
        /// Cancels the order
        /// </summary>
        public void Cancel()
        {
            if (_klarnaId == String.Empty)
            {
                throw new Exception("Cannot cancel an order that has not been created");
            }
            HttpWebResponse response =
                SendRequest(new Uri(GetBaseUrl() + "/v1/" + _config.MerchantId + "/orders/" + _klarnaId + "/cancel"),
                    "POST");
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Something went wrong with the cancellation");
            }
        }
        public void SetTextMessage(string text)
        {
            if (text.IndexOf("{URL}", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var index = text.IndexOf("{URL}", StringComparison.OrdinalIgnoreCase);
                var firstpart = text.Substring(0, index);
                var secondpart = text.Substring(index + 5);
                text = firstpart + "{url}" + secondpart;
            }
            if (text.Contains("{url}") == false)
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
            if (_config.Environment == MerchantConfig.Server.Live)
            {
                return "https://buy.klarna.com";
            }
            return "https://buy.playground.klarna.com";
        }
    }
}
