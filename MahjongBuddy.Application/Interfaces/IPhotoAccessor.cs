using MahjongBuddy.Application.Photos;
using Microsoft.AspNetCore.Http;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile file);

        string DeletePhoto(string publicId);
    }
}
