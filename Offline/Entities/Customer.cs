
namespace Klarna.Offline.Entities
{
    public class Customer
    {
        public string country;
        public string street_address;
        public string city;
        public string phone;
        public string given_name;
        public string family_name;
        public string postal_code;
        public string email;
        public string FirstName
        {
            get { return given_name; }
        }
        public string LastName
        {
            get { return family_name; }
        }
        public string Address
        {
            get { return street_address; }
        }
        public string Postal
        {
            get { return postal_code; }
        }
        public string City
        {
            get { return city; }
        }
        public string Email
        {
            get { return email; }
        }
        
    }
}
