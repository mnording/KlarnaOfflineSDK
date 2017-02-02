using System;
using System.Globalization;
using Klarna.Offline;
using Klarna.Offline.Entities;
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
            OfflineOrder t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+46700024576",
                "ref");
            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void MustValidateNorwegianPhone()
        {
            OfflineOrder t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+4790000000",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+4740000000",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+4759000000",
                "ref");
            Assert.IsNotNull(t);

        }

        [TestMethod]
        public void MustValidateFinnishPhone()
        {
            OfflineOrder t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+358401234567",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+358501234",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+35845733654578",
                "ref");
            Assert.IsNotNull(t);
            t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+35850457894",
                "ref");
            Assert.IsNotNull(t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnToLongFIPhone()
        {
            var offlineOrder = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+3584012345673332231312312",
                "ref");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnToShortFIPhone()
        {
            OfflineOrder t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("fi-fi"), "EUR", "FI",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+35840123",
                "ref");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnWrongSEhone()
        {
            OfflineOrder t = new OfflineOrder(new Cart(),
                new MerchantConfig(CultureInfo.CreateSpecificCulture("sv-se"), "SEK", "SE",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+4670002457622",
                "ref");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MustThrowErrorOnWrongNOPhone()
        {
            OfflineOrder t = new OfflineOrder(new Cart(),
                new Klarna.Offline.Entities.MerchantConfig(CultureInfo.CreateSpecificCulture("nb-no"), "NOK", "NO",
                    "testid", "testid", MerchantConfig.Server.Test),
                "test",
                "+47900000000",
                "ref");
        }
    }
}
