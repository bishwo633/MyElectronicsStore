using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoice { get; set; }
        public int InvoiceId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double SubAmount { get; set; }
    }
}