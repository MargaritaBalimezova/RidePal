using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IPixabayServices
    {
        Task<string> GetImageURL();
    }
}