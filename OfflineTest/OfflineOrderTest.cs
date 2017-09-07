using System;
using System.Collections.Generic;
using System.Globalization;
using Klarna.Offline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Entities;
using MerchantConfig = Klarna.Offline.Entities.MerchantConfig;

namespace OfflineTest
{
    [TestClass]
    public class OfflineOrderTest
    {
        [TestMethod]
        public void MustValidateSwedishPhone()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(), 
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46700024576",
                "ref");
            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void MustValidateNorwegianPhone()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", Server.Playground),
                "test",
                "+4790000000",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", Server.Playground),
                "test",
                "+4740000000",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", Server.Playground),
                "test",
                "+4759000000",
                "ref");
            Assert.IsNotNull(t);

        }

        [TestMethod]
        public void MustValidateFinnishPhone()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+358401234567",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+358501234",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+35845733654578",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+35850457894",
                "ref");
            Assert.IsNotNull(t);
        }

        [TestMethod]
        [ExpectedException(typeof(PhoneNumbers.NumberParseException))]
        public void MustThrowErrorOnToLongFiPhone()
        {
            new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+3584012345673332231312312",
                "ref");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnToShortFiPhone()
        {
            new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", Server.Playground),
                "test",
                "+35842",
                "ref");
        }

        [TestMethod]
        [ExpectedException(typeof(PhoneNumbers.NumberParseException))]
        public void MustThrowErrorOnWrongSEhone()
        {
            new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46700024576223423422",
                "ref");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnWrongNoPhone()
        {
            new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", Server.Playground),
                "test",
                "+439000000000000",
                "ref");
        }

        [TestMethod]
        public void PostBackUrlHasToBeHttps()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46729922222",
                "ref", new Uri("https://www.test.com"));
            Assert.IsNotNull(t);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PostBackUrlCannotBeHttp()
        {
           new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46729922222",
                "ref", new Uri("http://www.test.com"));
        }

        [TestMethod]
        public void CanSetOwnText()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46729922222",
                "ref", new Uri("https://www.test.com"));
            var smsText = "THis is my own URL with a {url} inside of it";
            t.SetTextMessage(smsText);
            Assert.AreEqual(smsText, t.sms_text);
        }

        [TestMethod]
        public void AutoAddingUrlPlaceholder()
        {
            OfflineOrder offlineOrder = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46729922222",
                "ref", new Uri("https://www.test.com"));
            var smsText = "This is my own text";
            offlineOrder.SetTextMessage(smsText);
            Assert.AreEqual(smsText + " {url}", offlineOrder.sms_text);
        }

        [TestMethod]
        public void CorrectingWrongPlaceholder()
        {
            OfflineOrder t = new OfflineOrder(new List<OrderLine>(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", Server.Playground),
                "test",
                "+46729922222",
                "ref", new Uri("https://www.test.com"));
            var smsText = "This is my own text{uRl}";
            t.SetTextMessage(smsText);
            Assert.AreEqual("This is my own text{url}", t.sms_text);
        }

        [TestMethod]
        public void CanCreateOrderOnId()
        {
           new OfflineOrder("04341105793e19059ed14b39622bfaeb09484541",
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testShared", "testid", Server.Playground));
        }
    }
}
