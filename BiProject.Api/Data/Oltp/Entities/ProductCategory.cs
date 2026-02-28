using System.ComponentModel.DataAnnotations;

namespace BiProject.Api.Data.Oltp.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
