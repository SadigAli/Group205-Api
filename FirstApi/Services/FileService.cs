namespace FirstApi.Services
{
    public class FileService
    {
        public async Task<string> FileUpload(string wwwroot,string folder,IFormFile file)
        {
            string filePath = Path.Combine(wwwroot, "uploads", folder);
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPath = Path.Combine(filePath, fileName);
            // C:/geksgjh/FirstApi/wwwroot/uploads/products/filename
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        public void FileDelete(string wwwroot,string file,string folder)
        {
            string fullPath = Path.Combine(wwwroot, "uploads", folder, file);
            // C:\Users\sadig\OneDrive\Desktop\Group205-Api\FirstApi\wwwroot\uploads\products\9e0910be-e1c1-4dc2-939f-a4433a15234d_qarpiz.jpg
            if (System.IO.File.Exists(fullPath)) 
            { 
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
