using Klarna;
using Klarna.Entities;
using Klarna.Offline;
using Klarna.Offline.Entities;
using System;
using System.Globalization;

namespace OfflineDemoApp
{
    public class KlarnaIntegration
    {
        public void StartOrderWithPolling()
        {
            // Setup a Config with the basic information on the merchant
            var config = new Klarna.Offline.Entities.MerchantConfig(CultureInfo.CurrentCulture, Currency.SEK, Country.SE, "sDOWW9d9oGZZ4bw", "7262");
            //Create cart and add items
            var cart = new Cart();
            //Defined methods for adding products
            cart.addProduct(new CartRow("Prod1234", "New Shoes for you", 1, 10000, 25));
            cart.addProduct(new CartRow("Prod1233", "New purple for you", 1, 10000, 25));
            //Defined method for adding Discount as a seperate row
            cart.addDiscount(new CartRow("discount-1", "Summer sales", 1, -1000,25));
            string phone = Console.ReadLine();

            OfflineOrder offlineOrder = new OfflineOrder(cart, config, "terminal", phone, "Merchant_OrderReference");
            offlineOrder.Create();
           
            //Fetching the status URL for polling
            var url = offlineOrder.GetStatusUrl();
            Console.WriteLine("Polling Data.....");
            OrderDetails details;
            do
            {
                //Get the order data for presenting
                details = offlineOrder.pollData(url);
            } while (details == null);
            Console.WriteLine(" ");
            Console.WriteLine("Order completed!");
            Console.WriteLine(" ");
            Console.WriteLine(details.customer.given_name + " " + details.customer.family_name);
            Console.WriteLine("Customer email: " + details.customer.email);
            Console.ReadLine();
        }
        public void StartOrderWithStatusUrl()
        {
            var config = new Klarna.Offline.Entities.MerchantConfig(CultureInfo.CurrentCulture, Currency.SEK, Country.SE, "sDOWW9d9oGZZ4bw", "7262");
            var cart = new Cart();
            cart.addProduct(new CartRow("Prod1234", "New Shoes for you", 1, 10000, 25));
            cart.addProduct(new CartRow("Prod1233", "New purple for you", 1, 10000, 25));
            cart.addDiscount(new CartRow("discount-1", "Summer sales", 1, -1000, 25));
            string phone = Console.ReadLine();
            var t = new OfflineOrder(cart, config, "terminal", phone, "1", new Uri("https://mockbin.org/bin/f53a5914-dadd-4ed4-90c0-b0e647b91d2b"));
            t.Create();
            OrderDetails details;
            Console.WriteLine("Enter Json from mockbin after order completion");
            var json = Console.ReadLine();
            details = Klarna.Offline.Helpers.JsonConverter.GetOrderFromString(@json);
            Console.WriteLine(" ");
            Console.WriteLine("Order completed!");
            Console.WriteLine(" ");
            Console.WriteLine(details.customer.FirstName + " " + details.customer.LastName);
            Console.WriteLine("Customer email: " + details.customer.Email);
        }
    }
}
