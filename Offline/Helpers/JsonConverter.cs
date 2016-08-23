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
            return details;
        }
    }
}
