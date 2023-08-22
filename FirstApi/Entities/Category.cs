using System.ComponentModel.DataAnnotations;

namespace FirstApi.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(50),MinLength(3),MaxLength(50)]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
