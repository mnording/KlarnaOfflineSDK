using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarna.Entities;

namespace Klarna.Offline.Entities
{
   public class OfflineOrderLine : OrderLine
    {
        public OfflineOrderLine(string name, string reference,int quantity, int price, int vat) : base(name, quantity, price, vat)
        {
            this.Reference = reference;
        }
    }
}
