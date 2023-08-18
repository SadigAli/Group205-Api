using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FirstApi.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required,StringLength(50)]
        public string Title { get; set; }
        [StringLength(5000),MaybeNull]
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}
