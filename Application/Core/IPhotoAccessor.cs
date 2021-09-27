using Microsoft.AspNetCore.Http;

namespace Application.Core
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult UploadImage(IFormFile file);
    }
}