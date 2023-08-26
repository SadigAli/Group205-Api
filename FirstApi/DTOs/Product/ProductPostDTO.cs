using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FirstApi.DTOs.Product
{
    public class ProductPostDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public int CategoryId { get; set; }
        public List<int>? ColorIds { get; set; }
        public IFormFile? File { get; set; }
    }
}
