using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class Cart
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}