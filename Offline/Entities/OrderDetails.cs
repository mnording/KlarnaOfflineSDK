namespace Klarna.Offline.Entities
{
    public class OrderDetails
    {
        public string id;
        public string message_code;
        public string message;
        public string invoice_id;
        public string reservation_id;
        public string invoice_pdf;
        public Customer customer;
    }
}