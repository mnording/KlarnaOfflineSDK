using System;
using System.Globalization;

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
        /// <param name="server">What server is being targeted?</param>
        public MerchantConfig(CultureInfo locale, string currency, string country, string sharedSecret,string merchantId,Server server)
        {
            SetCommonParams(locale,currency,country,sharedSecret,merchantId,server);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale">The locale of the purchase</param>
        /// <param name="currency">The currency of the purchase ISO 4217</param>
        /// <param name="country">2 letter country code ISO 3166-1 alpha-2</param>
        /// <param name="sharedSecret">The sharedsecret between you and Klarna</param>
        /// <param name="merchantId">The merchant ID provided by Klarna</param>
        /// <param name="server">What server is being targeted?</param>
        public MerchantConfig(CultureInfo culture, string sharedSecret, string merchantId, Server server)
        {
            var region = new RegionInfo(culture.LCID);
            var country = region.TwoLetterISORegionName;
            var currency = region.ISOCurrencySymbol;
            SetCommonParams(culture, currency, country, sharedSecret, merchantId, server);
        }

        private void SetCommonParams(CultureInfo locale, string currency, string country, string sharedSecret, string merchantId, Server server)
        {
            VerifyCountries(currency, country);
            this.locale = locale.Name;
            purchaseCountry = country;
            purchaseCurrency = currency;
            this.merchantId = merchantId;
            this.sharedSecret = sharedSecret;
            this.server = server;

        }
        private void VerifyCountries(string currency, string country)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            if (country == null) throw new ArgumentNullException(nameof(country));
            if (country != "SE" && country != "NO" && country != "FI")
            {
                throw new ArgumentException("Country not supported");
            }
            if (currency != "SEK" && currency != "EUR" && currency != "NOK")
            {
                throw new ArgumentException("Currency not supported");
            }
            if (country == "SE" && currency != "SEK")
            {
                throw new ArgumentException("Currency/Country missmatch");
            }
            if (country == "FI" && currency != "EUR")
            {
                throw new ArgumentException("Currency/Country missmatch");
            }
             if (country == "NO" && currency != "NOK")
            {
                throw new ArgumentException("Currency/Country missmatch");
            }


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
        public Server Environment
        {
            get { return server; }
        }
    }
}
