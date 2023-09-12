using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Contracts
{
    public interface IFileRepository
    {
        public Task<string> FileUpload(string folder,IFormFile file);
        public void DeleteFile(string folder,string file);
    }
}
