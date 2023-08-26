using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FirstApi.DTOs.Product
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public List<string> Colors { get; set; }
    }
}
