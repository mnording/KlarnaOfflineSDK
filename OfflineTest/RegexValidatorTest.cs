using System;
using Klarna;
using Klarna.Entities;
using Klarna.Offline.Entities;
using Klarna.Offline.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OfflineTest
{
    [TestClass]
    public class RegexValidatorTest
    {
      
        [TestMethod]
        public void CorrectStringIsCorrect()
        {
            RegexValidator.Validate("pianwdpionpina pdfanw dj apåwmdp anwp []odi nawa");
        }
        [TestMethod]
        public void WillRemoveIncorrectChar()
        {
           string s =  RegexValidator.RemoveInvalidChars("pianwdpionpina pdfanw dj ' ' ' ' ´´ `apåwmdp anwpodi nawa");
            Assert.AreEqual("pianwdpionpina pdfanw dj      apåwmdp anwpodi nawa",s);
        }

        [TestMethod]
        public void WillReplaceCorrectly()
        {
            string s = RegexValidator.ReplaceInvalidChars("This hyphen´ should be replaced with an a","a");
            Assert.AreEqual("This hyphena should be replaced with an a",s);
        }

        [TestMethod]
        public void WillValidateCorrectCart()
        {
         var cart = new Cart();
            cart.addProduct(new CartRow("test", "testname", 2, 2000, VatMode.IncVat, 25));
            RegexValidator.ValidateCartItems(cart);
        }
        [TestMethod]
        public void WillValidateCorrectCustomer()
        {
            var customer = new Customer();
            customer.given_name = "awda";
            customer.family_name = "awd";
            customer.street_address = "awda";
                RegexValidator.ValidateCustomer(customer);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillRejectInvalidCart()
        {
            var cart = new Cart();
            cart.addProduct(new CartRow("test", "testn' '`ame", 2, 2000, VatMode.IncVat, 25));
            RegexValidator.ValidateCartItems(cart);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillRejectInvalidCustomer()
        {

            var customer = new Customer
            {
                given_name = "awda",
                family_name = "aw'^'*`d",
                street_address = "awda"
            };
            RegexValidator.ValidateCustomer(customer);
        }
    }
}
