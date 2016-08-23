using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarna.Entities
{
    public class Order
    {
        Cart cart;
        MerchantConfig config;
        public Order(Cart cart, MerchantConfig config)
        {
            this.cart = cart;
            this.config = config;
    }
    }
    
}
