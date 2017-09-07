using System.Globalization;
using Klarna.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantConfig = Klarna.Offline.Entities.MerchantConfig;

namespace OfflineTest
{
    [TestClass]
    public class FinnishMerchantConfigTest
    {
        [TestMethod]
        public void MustBeAbleToInitWithFinland()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI", "test", "test",
                Server.Live);
            Assert.AreNotEqual(null, t);
        }

        /*####################### 2nd Construct Test ######################### */

        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingCountry()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                Server.Live);
            Assert.AreEqual("FI", t.Country);
        }
        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingCurrency()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                Server.Live);
            Assert.AreEqual("EUR", t.Currency);
        }
        [TestMethod]
        public void MustBeAbleToInitWithFinlandSettingLocale()
        {
            var t = new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "test", "test",
                Server.Live);
            Assert.AreEqual("fi-FI", t.Locale);
        }
    }

}
