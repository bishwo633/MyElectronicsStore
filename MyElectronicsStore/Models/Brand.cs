using System.ComponentModel.DataAnnotations;

namespace MyElectronicsStore.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name ="Brand")]
        public string Name { get; set; }
    }
}