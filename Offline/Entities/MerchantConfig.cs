using Klarna.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarna.Offline.Entities
{
   public class MerchantConfig:Klarna.Entities.MerchantConfig
    {
        string locale;
        string purchaseCurrency;
        string purchaseCountry;
        string merchantId;
        string sharedSecret;
        Server server;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale">The locale of the purchase</param>
        /// <param name="currency">The currency of the purchase ISO 4217</param>
        /// <param name="country">2 letter country code ISO 3166-1 alpha-2</param>
        /// <param name="sharedSecret">The sharedsecret between you and Klarna</param>
        /// <param name="merchantId">The merchant ID provided by Klarna</param>
        public MerchantConfig(CultureInfo locale, string currency, string country, string sharedSecret,string merchantId,Server server)
        {
            if(country != "SE")
            {
                throw new ArgumentException("Country not supported");
            }
            if (currency != "SEK")
            {
                throw new ArgumentException("Currency not supported");
            }
            this.locale = locale.Name;
            purchaseCountry = country;
            purchaseCurrency = currency;
            this.merchantId = merchantId;
            this.sharedSecret = sharedSecret;
            this.server = server;
        }
        public string Currency
        {
            get { return purchaseCurrency; }
        }
        public string Country
        {
            get { return purchaseCountry; }
        }
        public string Locale
        {
            get { return locale; }
        }
        public string SharedSecret
        {
            get { return sharedSecret; }
        }
        public string MerchantId
        {
            get { return merchantId; }
        }
        public enum Server {  Test,Live }
        public Server Enviournment
        {
            get { return server; }
        }
    }
}
