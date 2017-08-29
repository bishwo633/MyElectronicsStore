using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class PaymentToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public string UserName { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}