namespace BodyProgress.Services.Common
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;

    public interface IUploadMediaService
    {
        public Task<string> UploadImage(Cloudinary cloudinary, IFormFile image, string name);

        public void DeleteImage(Cloudinary cloudinary, string name);

        bool IsImage(IFormFile postedFile);
    }
}
