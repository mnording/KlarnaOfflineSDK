using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarna.Entities
{
    public class CartRow
    {
        string artno;
        string artname;
        int price;
        int vatPercentage;
        int quant;
        VatMode vatMode;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artno">Unique SKU</param>
        /// <param name="name">Friendly name</param>
        /// <param name="quantity">Number of products</param>
        /// <param name="price">Price in cents</param>
        /// <param name="vatMode">Inc or Ex vat</param>
        /// <param name="vatPercentage">How many percentages VAT should be set</param>
        public CartRow(string artno,string name, int quantity, int price,VatMode vatMode,int vatPercentage)
        {
            
            this.artno = artno;
            this.artname = name;
            this.price = price;
            this.vatPercentage = vatPercentage;
            this.quant = quantity;
            this.vatMode = vatMode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artno">Unique SKU</param>
        /// <param name="name">Friendly name</param>
        /// <param name="quantity">Number of products</param>
        /// <param name="price">Price in cents</param>
        /// <param name="vatPercentage">How many percentages VAT should be set</param>
        public CartRow(string artno, string name, int quantity, int price,  int vatPercentage) : this(artno,name,quantity,price,VatMode.IncVat,vatPercentage)
        {
        }
        public int unit_price
        {
            get
            {
                return price;
            }
        }
        public string name
        {
            get
            {
                return this.artname;
            }
        }
        public int tax_rate
        {
            get
            {
                return (vatPercentage * 100 );
            }
        }
        public string reference
        {
            get { return artno; }
        }
        public int quantity
        {
            get
            {
                return quant;
            }
        }
    }
}
