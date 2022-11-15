using RidePal.Services.DTOModels;
using System.Threading.Tasks;

namespace RidePal.Web.Helpers
{
    public interface IAuthHelper
    {
        Task<LoginUserDTO> TryLogin(string email, string password);
    }
}