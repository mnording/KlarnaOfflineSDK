using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Offline.Entities;

namespace OfflineTest
{
    [TestClass]
    public class SwedishMerchantConfigTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCountry()
        {
            new MerchantConfig(new CultureInfo(1), "SEK", "DE", "test", "test",
                MerchantConfig.Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSweden()
        {
            var t = new MerchantConfig(new CultureInfo(1), "SEK", "SE", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCurrency()
        {
            new MerchantConfig(new CultureInfo(1), "EUR", "SE", "test", "test",
                MerchantConfig.Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSwedishSek()
        {
            var t = new MerchantConfig(new CultureInfo(1), "SEK", "SE", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }

        /*####################### 2nd Construct Test ######################### */

        [TestMethod]
        public void MustBeAbleToInitWithSwedenSettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("SE", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSwedenOnlySettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("SEK", t.Currency);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSwedenOnlySettingLocale()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("sv-SE", t.Locale);
        }

}

}
