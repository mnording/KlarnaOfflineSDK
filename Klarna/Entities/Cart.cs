using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarna.Entities
{
    public class Cart
    {
        List<CartRow> items;
        public Cart()
        {          
                items = new List<CartRow>();
        }
        public void addProduct(CartRow product)
        {
            if (product.unit_price <= 0)
            {
                throw new ArgumentException("Price cannot be 0 on a product");
            }
            items.Add(product);
        }
        public void addDiscount(CartRow discount)
        {
            if (discount.unit_price >= 0)
            {
                throw new ArgumentException("price cannot be over 0 on a discount");
            }
            items.Add(discount);
        }
        public List<CartRow> OrderLines
        {
            get
            {
                return items;
            }
        }
    }
}
