using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Offline.Entities;

namespace OfflineTest
{
    [TestClass]
    public class MerchantConfigTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCountry()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "SEK", "DE", "test", "test",
                MerchantConfig.Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSweden()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "SEK", "SE", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }

        [TestMethod]
        public void MustBeAbleToInitWithFinland()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "EUR", "FI", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }

        [TestMethod]
        public void MustBeAbleToInitWithNorway()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "NOK", "NO", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustNotBeAbleToInitWrongCurrency()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "EUR", "SE", "test", "test",
                MerchantConfig.Server.Live);
        }

        [TestMethod]
        public void MustBeAbleToInitWithSwedishSEK()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "SEK", "SE", "test", "test",
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

        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("FI", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("EUR", t.Currency);
        }
        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingLocale()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("fi-FI", t.Locale);
        }

        [TestMethod]
        public void MustBeAbleToInitWithNorwaySettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("NO", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithNorwaySettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreEqual("NOK", t.Currency);
        }
    
    [TestMethod]
    public void MustBeAbleToInitWithNorwaySettingLocale()
    {
        var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
            MerchantConfig.Server.Live);
        Assert.AreEqual("nb-NO", t.Locale);
    }
}

}
