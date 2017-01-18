using Klarna.Offline.Entities;
using Newtonsoft.Json;
using System.IO;

namespace Klarna.Offline.Helpers
{
    public static class JsonConverter
    {
        public static OrderDetails GetOrderFromString(string input)
        {
            var jsreader = new JsonTextReader(new StringReader(input));
            var details = new JsonSerializer().Deserialize<OrderDetails>(jsreader);
            if(details.invoice_id == null && details.reservation_id == null)
            {
                throw new InvalidDataException("Missing invoice/reservation ID");
            }
            if(details.customer == null)
            {
                throw new InvalidDataException("Missing customer object");
            }
            if(details.customer.City == null || details.customer.FirstName == null || details.customer.LastName == null)
            {
                throw new InvalidDataException("Missing customer details");
            }
            return details;
        }
    }
}
