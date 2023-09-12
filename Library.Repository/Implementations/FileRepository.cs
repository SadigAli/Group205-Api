using Library.Repository.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly IWebHostEnvironment _env;
        public FileRepository(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void DeleteFile(string folder, string file)
        {
            string fullPath = Path.Combine(_env.WebRootPath, "uploads", folder, file);
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<string> FileUpload(string folder, IFormFile file)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "books");
            string filePath = Guid.NewGuid() + "_" + file.FileName;
            string fullPath = Path.Combine(folderPath, filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
