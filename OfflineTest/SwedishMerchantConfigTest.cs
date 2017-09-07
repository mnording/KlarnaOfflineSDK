using System;
using System.Globalization;
using Klarna.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantConfig = Klarna.Offline.Entities.MerchantConfig;

namespace OfflineTest
{
    [TestClass]
    public class SwedishMerchantConfigTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCountry()
        {
            new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "DE", "test", "test",
                Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSweden()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE", "test", "test",
                Server.Live);
            Assert.AreNotEqual(null, t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCurrency()
        {
            new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "EUR", "SE", "test", "test",
                Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSwedishSek()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE", "test", "test",
                Server.Live);
            Assert.AreNotEqual(null, t);
        }

        /*####################### 2nd Construct Test ######################### */

        [TestMethod]
        public void MustBeAbleToInitWithSwedenSettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                Server.Live);
            Assert.AreEqual("SE", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSwedenOnlySettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                Server.Live);
            Assert.AreEqual("SEK", t.Currency);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSwedenOnlySettingLocale()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                Server.Live);
            Assert.AreEqual("sv-SE", t.Locale);
        }

}

}
