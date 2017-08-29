using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        [Range(1, 1000)]
        public decimal Price { get; set; }
    }
}