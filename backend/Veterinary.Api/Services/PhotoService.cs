using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Veterinary.Api.Extensions;
using Veterinary.Application.Services;

namespace Veterinary.Api.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PhotoService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadPhoto(string folderName, string key, IFormFile photo)
        {
            string path = null;
            if (photo != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", folderName);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string newFileName = $"{key}-{Guid.NewGuid().ToString("N")}.png";

                path = Path.Combine("Images", folderName, newFileName);

                using (var fileStream = new FileStream(Path.Combine(folder, newFileName), FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
            }
            return httpContextAccessor.GetApplicationUrl() + "\\" + path;
        }

        public bool RemovePhoto(string path)
        {
            var localPath = path.Substring(path.IndexOf("Images"), path.Length);
            if (File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, localPath)))
                File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, localPath));

            return !File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, localPath));

        }



    }
}
