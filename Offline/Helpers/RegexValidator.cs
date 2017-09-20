using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Klarna.Entities;
using Klarna.Offline.Entities;
using Newtonsoft.Json.Schema;

namespace Klarna.Offline.Helpers
{
    public static class RegexValidator
    {
        static Regex r = new Regex(@"^[\wåäöæøÅÄÖÆØ@_ \-\.\s\,\:\@\(\)\[\]\{\}]*$");

        public static void Validate(string input)
        {
            try
            {

                if (!r.IsMatch(input))
                {
                    throw new ArgumentException(input + " is not a valid string according to the API");
                }
            }
            catch (System.Exception e)
            {
                throw new ArgumentException(input + " is not a valid string according to the API");
            }
           
        }
        public static string RemoveInvalidChars(string input)
        {
           return ReplaceInvalidChars(input);
        }
        public static string ReplaceInvalidChars(string input,string replace = "")
        {
            var chars = input.ToCharArray();

            var result = "";
            foreach (char c in chars)
            {
                if (r.IsMatch(c.ToString()))
                {
                    result += c.ToString();
                }
                else
                {
                    result += replace;
                }
            }

            return result;
        }

        public static void ValidateCartItems(List<OrderLine> c)
        {
            foreach (OrderLine row in c)
            {
                
                Validate(row.Name);
                if (row.Reference != null)
                {
                    Validate(row.Reference);
                }
               
            }
        }
        public static void ValidateCustomer(Customer customer)
        {
            Validate(customer.Address);
            Validate(customer.City);
            Validate(customer.Email);
            Validate(customer.FirstName);
            Validate(customer.Postal);
            Validate(customer.LastName);
        }
    }
}
