using Microsoft.AspNetCore.Authentication;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers
{
    public class AddClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserServices userService;

        public AddClaimsTransformation(IUserServices userService)
        {
            this.userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            try
            {
                var clone = principal.Clone();
                var newIdentity = (ClaimsIdentity)clone.Identity;

                // Support AD and local accounts
                var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (email == null)
                {
                    return principal;
                }

                // Get user from database

                var user = await userService.GetUserDTOByEmailAsync(email.Value);
                if (user == null)
                {
                    return principal;
                }

                if (!string.IsNullOrEmpty(user.AccessToken))
                {
                    var claim = new Claim("AuthToken", user.AccessToken);
                    newIdentity.AddClaim(claim);
                }

                //new Claim("Image", user.ImagePath),
                var imgClaim = principal.Claims.FirstOrDefault(x => x.Type == "Image");
                if (user.ImagePath != imgClaim.Value)
                {
                    newIdentity.RemoveClaim(imgClaim);
                    newIdentity.AddClaim(new Claim("Image", user.ImagePath));
                }

                return clone;
            }
            catch (Exception)
            {
                return principal;
            }
        }
    }
}