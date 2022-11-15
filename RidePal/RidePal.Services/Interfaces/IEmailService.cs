using RidePal.Services.Models;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}