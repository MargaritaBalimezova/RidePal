using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IAWSCloudStorageService
    {
        Task Upload(IFormFile file, string fileName);

        Task<string> UploadPlaylistImage(byte[] bytes, string playlistName);
    }
}