using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Offline.Entities;

namespace OfflineTest
{
    [TestClass]
    public class NorwegianMerchantConfigTest
    {
        [TestMethod]
        public void MustBeAbleToInitWithNorway()
        {
            var t = new MerchantConfig(new System.Globalization.CultureInfo(1), "NOK", "NO", "test", "test",
                MerchantConfig.Server.Live);
            Assert.AreNotEqual(null, t);
        }
        /*####################### 2nd Construct Test ######################### */

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
