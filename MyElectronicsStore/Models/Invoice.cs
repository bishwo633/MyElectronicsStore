using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public double TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public virtual IEnumerable<InvoiceDetail> Invoices { get; set; }
    }
}