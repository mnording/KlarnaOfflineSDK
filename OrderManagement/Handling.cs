using Klarna.Api;
using Klarna.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement
{
    public class Handling
    {
        Configuration configuration;
        Api api;
        public Handling(MerchantConfig config)
        {
            configuration = new Configuration(
            Klarna.Api.Country.Code.SE, Language.Code.SV, Klarna.Api.Currency.Code.SEK,
            Klarna.Api.Encoding.Sweden)
            {
                Eid = 0,
                Secret = "sharedsecret",
                IsLiveMode = false
            };

            api = new Api(configuration);
        }
       public void Activate(string reservationNumber)
        {
            api.Activate(reservationNumber);
        }
        public void Refund(string invoiceNumber)
        {
            api.CreditInvoice(invoiceNumber);
        }
        public void Refund(string invoiceNumber, List<CartRow> artnos)
        {
            foreach(CartRow art in artnos)
            {
                api.AddArticleNumber(art.quantity,art.reference);
            }
            api.CreditPart(invoiceNumber);
        }
        public void ReturnAmount(string invoiceNumber,int amount,int vatpercentage)
        {
            var amountAsDouble = Convert.ToDouble(amount / 100);
            var vatAsDouble = Convert.ToDouble(vatpercentage / 100);
            api.ReturnAmount(invoiceNumber, amountAsDouble, vatAsDouble);
        }
        [Obsolete("ActivateReservation is deprecated, please use activate instead.", true)]
        public void ActivateReservation()
        {
             
        }
    }
}
