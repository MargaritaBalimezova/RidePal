using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Web.Helpers;
using RidePal.Web.Models;
using RidePal.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthHelper authHelper;
        private readonly IUserServices userService;
        private readonly IMapper mapper;

        public AuthController(IAuthHelper authHelper, IUserServices userService, IMapper mapper)
        {
            this.authHelper = authHelper;
            this.userService = userService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            var email = claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).FirstOrDefault();

            if (await userService.IsExistingAsync(email))
            {
                var user = await userService.GetUserDTOByEmailAsync(email);

                var cookieClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName),
                    //new Claim("Image", user.ImagePath)
                };

                if (user.RoleId == 1)
                {
                    cookieClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    cookieClaims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var claimsIdentity = new ClaimsIdentity(
                    cookieClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                     new ClaimsPrincipal(claimsIdentity)
                );


                return RedirectToAction("Index", "Home");
            }
            await this.GoogleRegister();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task GoogleRegister()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            var email = claims.Where(c => c.Type == ClaimTypes.Email)
               .Select(c => c.Value).SingleOrDefault();
            var firstName = claims.Where(c => c.Type == ClaimTypes.GivenName)
                   .Select(c => c.Value).SingleOrDefault();
            var lastName = claims.Where(c => c.Type == ClaimTypes.Surname)
                   .Select(c => c.Value).SingleOrDefault();
            var username = email.Split('@');

            while (await userService.IsExistingUsernameAsync(username[0]))
            {
                Random rnd = new Random();
                username[0] = username[0] + rnd.Next(1, 100);
            }

            var userDTO = new UpdateUserDTO
            {
                Username = username[0],
                FirstName = firstName,
                LastName = lastName,
                Email = email,       
                IsEmailConfirmed = true,
                IsGoogleAccount = true
            };
                        
            await userService.PostAsync(userDTO);           
        }


        [HttpGet]
        public IActionResult Register()
        {
            var register = new CreateUserViewModel();
            return this.View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (await userService.IsExistingAsync(model.Email))
            {
                this.ModelState.AddModelError("Email", "User with this email address already exists.");
            }
            if (await userService.IsExistingUsernameAsync(model.Username))
            {
                this.ModelState.AddModelError("Username", "User with this username already exists.");
            }
            try
            {
                var userDTO = new UpdateUserDTO
                {
                    Password = model.Password,
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    //TODO Remove below line after implementing Email Confirmation
                    IsEmailConfirmed  = true,
                    IsGoogleAccount = false,
                    ImagePath = "defaultphoto.jpg"
                };

                var newUser = await userService.PostAsync(userDTO);

            }
            catch (Exception)
            {
                this.ModelState.AddModelError("Password", "Incorrect password.");
                return this.View(model);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var login = new LoginViewModel();
            return this.View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!await userService.IsExistingAsync(model.Credential) && !await userService.IsExistingUsernameAsync(model.Credential))
            {
                this.ModelState.AddModelError("Credential", "Incorrect combination of email/username and password.");
                return this.View(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var user = await authHelper.TryLogin(model.Credential, model.Password);

                if (user == null)
                {
                    this.ModelState.AddModelError("IsEmailConfirmed", "You have to confirm your email.");
                    return this.View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName),
                    new Claim("Image", user.ImagePath)
                };

                if (user.RoleId == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var AuthProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddDays(14)
                };


                if (model.RememberMe == true)
                {
                    AuthProperties.IsPersistent = true;
                }

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    AuthProperties
                );
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("Password", "Incorrect combination of email and password.");
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var cookie = Request.Cookies[""];

            return this.RedirectToAction("Index", "Home");
        }


    }
}
