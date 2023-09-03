using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Author
{
    public class AuthorPostDTO
    {
        [MaxLength(20),MinLength(3)]
        public string Firstname { get; set; }
        [MaxLength(20), MinLength(3)]
        public string Lastname { get; set; }
    }
}
