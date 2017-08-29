using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyElectronicsStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}