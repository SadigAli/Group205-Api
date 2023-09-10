using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class Author : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<BookAuthor> BookAuthors { get; set; }

        public string GetFullName()
        {
            return $"{Firstname} {Lastname}";
        }
    }
}
