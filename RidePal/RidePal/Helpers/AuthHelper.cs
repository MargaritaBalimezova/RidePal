using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RidePal.Web.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IUserServices userServices;
        private readonly IMapper mapper;
        private static readonly string ErrorMassage = "Invalid authentication info";

        public AuthHelper(IUserServices userServices, IMapper mapper)
        {
            this.userServices = userServices;
            this.mapper = mapper;
        }

        public async Task<LoginUserDTO> TryLogin(string credential, string password)
        {
            try
            {
                var user = new LoginUserDTO();
                 
                if (credential.Contains('@'))
                {
                    var userDTO = await userServices.GetUserDTOByEmailAsync(credential);
                     user = mapper.Map<LoginUserDTO>(userDTO);
                }
                else
                {
                    var userDTO = await userServices.GetUserDTOAsync(credential);
                    user = mapper.Map<LoginUserDTO>(userDTO);
                }

                if (user.IsEmailConfirmed)
                {

                    var passHasher = new PasswordHasher<User>();
                    var result = passHasher.VerifyHashedPassword(mapper.Map<User>(user), user.Password, password);

                    if (result != PasswordVerificationResult.Success)
                    {
                        throw new AuthenticationException();
                    }

                    return user;
                }

                user = null;
                return user;
            }
            catch (Exception)
            {
                throw new Exception(ErrorMassage);
            }
        }
    }
}
