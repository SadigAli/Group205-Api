using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Author
{
    public class AuthorGetDTO : AuthorPostDTO
    {
        public int Id { get; set; }
    }
}
