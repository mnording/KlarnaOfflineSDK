using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klarna.Offline.Helpers;
using Klarna.Offline.Entities;
using System.IO;

namespace OfflineTest
{
    [TestClass]
    public class JsonConverterTest
    {
        [TestMethod]
        public void MustValidateCorrectOrderInformation()
        {
            OrderDetails t = JsonConverter.GetOrderFromString("{" +
              "'id': '037f5998a5ed1371c9098542acd6e4c5214dc52a'," +
              "'message_code': 200," +
              "'message': 'Din transaktion är godkänd, du får en faktura från Klarna'," +
              "'invoice_id': '41512000019100214'," +
              "'invoice_pdf': 'https://klarna.com/invoice/111111.pdf'," +
              "'customer': {" +
                            "'country': 'se'," +
                "'street_address': 'VÄSTMANNAGATAN'," +
                "'city': 'STOCKHOLM'," +
                "'phone': '070 000 00 00'," +
                "'given_name': 'Testperson-se'," +
                "'family_name': 'Approved'," +
                "'postal_code': '11100'," +
                "'email': 'testperson@klarna.com'" +
              "}}");
            Assert.AreNotEqual(null, t);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void MustNotValidateIncorrectOrderCustomerInformation()
        {
            OrderDetails t = JsonConverter.GetOrderFromString("{" +
              "'id': '037f5998a5ed1371c9098542acd6e4c5214dc52a'," +
              "'message_code': 200," +
              "'message': 'Din transaktion är godkänd, du får en faktura från Klarna'," +
              "'invoice_id': '41512000019100214'," +
              "'invoice_pdf': 'https://klarna.com/invoice/111111.pdf'," +
              "'customer': {" +
                            "'country': 'se'," +
                            "'street_address': 'VÄSTMANNAGATAN'," +
                            "'city2': 'STOCKHOLM'," +
                            "'phone': '070 000 00 00'," +
                            "'given_name': 'Testperson-se'," +
                            "'family_name': 'Approved'," +
                            "'postal_code': '11100'," +
                            "'email': 'testperson@klarna.com'" +
              "}}");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void MustNotValidateMissingOrderCustomerInformation()
        {
            OrderDetails t = JsonConverter.GetOrderFromString("{" +
              "'id': '037f5998a5ed1371c9098542acd6e4c5214dc52a'," +
              "'message_code': 200," +
              "'message': 'Din transaktion är godkänd, du får en faktura från Klarna'," +
              "'invoice_id': '41512000019100214'," +
              "'invoice_pdf': 'https://klarna.com/invoice/111111.pdf'," +
              "}");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void MustNotValidatemMissingInvoice()
        {
            OrderDetails t = JsonConverter.GetOrderFromString("{" +
              "'id': '037f5998a5ed1371c9098542acd6e4c5214dc52a'," +
              "'message_code': 200," +
              "'message': 'Din transaktion är godkänd, du får en faktura från Klarna'," +
              "'invoice_pdf': 'https://klarna.com/invoice/111111.pdf'," +
              "'customer': {" +
                            "'country': 'se'," +
                            "'street_address': 'VÄSTMANNAGATAN'," +
                            "'city': 'STOCKHOLM'," +
                            "'phone': '070 000 00 00'," +
                            "'given_name': 'Testperson-se'," +
                            "'family_name': 'Approved'," +
                            "'postal_code': '11100'," +
                            "'email': 'testperson@klarna.com'" +
              "}}");
        }
    }
}
