using System.ComponentModel.DataAnnotations;

namespace BiProject.Api.Data.Oltp.Entities
{
    public class ProductDescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
