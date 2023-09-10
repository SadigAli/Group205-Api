using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public string Image { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public List<BookAuthor> BookAuthors { get; set; }
        public Genre Genre { get; set; }
    }
}
