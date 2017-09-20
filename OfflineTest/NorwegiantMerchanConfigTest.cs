using System.Globalization;
using Klarna;
using Klarna.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantConfig = Klarna.Offline.Entities.MerchantConfig;

namespace OfflineTest
{
    [TestClass]
    public class NorwegianMerchantConfigTest
    {
        [TestMethod]
        public void MustBeAbleToInitWithNorway()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO", "test", "test",
                Server.Live);
            Assert.AreNotEqual(null, t);
        }
        /*####################### 2nd Construct Test ######################### */

        [TestMethod]
        public void MustBeAbleToInitWithNorwaySettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
                Server.Live);
            Assert.AreEqual("NO", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithNorwaySettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
                Server.Live);
            Assert.AreEqual("NOK", t.Currency);
        }

        [TestMethod]
        public void MustBeAbleToInitWithNorwaySettingLocale()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "test", "test",
                Server.Live);
            Assert.AreEqual("nb-NO", t.Locale);
        }
    }

}
