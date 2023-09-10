using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Book
{
    public class GetBookDTO
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public List<string> Authors { get; set; }
    }
}
