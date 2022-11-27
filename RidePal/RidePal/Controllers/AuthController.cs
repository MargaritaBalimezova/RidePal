using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
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

        #region Login Google

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

                if (user.IsBlocked && DateTime.Compare(DateTime.Now, user.LastBlockTime.AddDays(7)) < 0)
                {
                    DateTime unblock = user.LastBlockTime.AddDays(7);
                    TimeSpan span = (unblock - DateTime.Now);

                    this.ModelState.AddModelError("IsBlocked", $"You were blocked on {user.LastBlockTime.ToString("dd/MM/yyyy hh:mm")}. Try again in {span.Days} days, {span.Hours} hours and {span.Minutes} minutes.");
                    return RedirectToAction("Login");
                }

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

        #endregion Login Google

        #region Register Google

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

        #endregion Register Google

        #region Register App
        [HttpGet]
        public IActionResult LoginPartial()
        {
            var login = new LoginViewModel();
            return this.PartialView("_LoginPartial",login);
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
                return this.View(model);
            }
            if (await userService.IsExistingUsernameAsync(model.Username))
            {
                this.ModelState.AddModelError("Username", "User with this username already exists.");
                return this.View(model);
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
                    IsGoogleAccount = false,
                    ImagePath = "default.jpg"
                };

                var newUser = await userService.PostAsync(userDTO);

                var user = new User
                {
                    Username = newUser.Username,
                };

                await userService.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email"))
                {
                    this.ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    this.ModelState.AddModelError("Username", ex.Message);
                }
                return this.View(model);
            }
            return View("ConfirmEmail", new EmailConfirmModel());
        }

        #endregion Register App

        #region Login App

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

                if (user.IsBlocked && DateTime.Compare(DateTime.Now, user.LastBlockTime.AddDays(7)) < 0)
                {
                    DateTime unblock = user.LastBlockTime.AddDays(7);
                    TimeSpan span = (unblock - DateTime.Now);

                    this.ModelState.AddModelError("IsBlocked", $"You were blocked on {user.LastBlockTime.ToString("dd/MM/yyyy hh:mm")}. Try again in {span.Days} days, {span.Hours} hours and {span.Minutes} minutes.");
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
                this.ModelState.AddModelError("Password", "Incorrect combination of email/username and password.");
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion Login App

        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var cookie = Request.Cookies[""];

            return this.RedirectToAction("Index", "Home");
        }

        #region Email actions

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // code here
                try
                {
                    var user = await userService.GetUserDTOByEmailAsync(model.Email);
                    await userService.GenerateForgotPasswordTokenAsync(mapper.Map<User>(user));

                    ModelState.Clear();
                    model.EmailSent = true;
                }
                catch (Exception)
                {
                    model.EmailSent = false;
                    return View();
                }
            }
            return View(model);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            EmailConfirmModel model = new EmailConfirmModel();

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                if (await userService.ConfirmEmailAsync(uid, token))
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await userService.GetUserDTOByEmailAsync(model.Email);

            if (user != null)
            {
                user.IsEmailConfirmed = model.IsConfirmed;

                if (user.IsEmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await userService.GenerateEmailConfirmationTokenAsync(mapper.Map<User>(user));
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong.");
            }
            return View(model);
        }

        [HttpGet("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await userService.ResetPasswordAsyncAsync(model);
                if (result)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }
                ModelState.AddModelError("", "Can't channge password");
            }
            return View(model);
        }

        #endregion Email actions
    }
}