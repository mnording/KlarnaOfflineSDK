using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Offline.Entities;

namespace OfflineTest
{
    [TestClass]
    public class MerchantConfigTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "Country not supported")]
        public void MustNotBeAbleToInitWrongCountry()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1),"SEK","DE","test","test",MerchantConfig.Server.Live);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSweden()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "SEK", "SE", "test", "test", MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "Currency not supported")]
        public void MustNotBeAbleToInitWrongCurrency()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "EUR", "SE", "test", "test", MerchantConfig.Server.Live);
        }
        [TestMethod]
        public void MustBeAbleToInitWithSwedishSEK()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "SEK", "SE", "test", "test", MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }
    }
}
