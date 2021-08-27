using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Veterinary.Application.Services
{
    public interface IPhotoService
    {
        Task<string> UploadPhoto(string folderName, string key, IFormFile photo);
    }
}
