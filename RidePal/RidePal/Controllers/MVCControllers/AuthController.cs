﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using RidePal.Web.Helpers;
using RidePal.Web.Models;
using RidePal.WEB.Helpers;
using RidePal.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RidePal.Web.Controllers
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

        #region Login Deezer

        public IActionResult LoginWithDeezer()
        {
            return Redirect($"https://connect.deezer.com/oauth/auth.php?app_id={ApiSecrets.DeezerId}&redirect_uri={Constants.RedirectUrlDeezer}");
        }

        [Authorize]
        public async Task<IActionResult> DeezerResponse(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync($"https://connect.deezer.com/oauth/access_token.php?app_id={ApiSecrets.DeezerId}&secret={ApiSecrets.DeezerSecret}&code={code}");
                var response = await result.Content.ReadAsStringAsync();
                var token = response.Split("access_token=").Last();
                var user = await userService.GetUserDTOByEmailAsync(this.User.Identity.Name);
                user.AccessToken = token;

                var userDTO = await userService.UpdateAsync(user.Id, mapper.Map<UpdateUserDTO>(user));

                return RedirectToAction("Index", "Home");
            }
        }

        #endregion Login Deezer

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
                    new Claim("Image", user.ImagePath)
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
            return RedirectToAction("GoogleResponse", "Auth");
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
                ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg",
                IsEmailConfirmed = true,
                IsGoogleAccount = true
            };

            await userService.PostAsync(userDTO);
        }

        #endregion Register Google

        #region Register App

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
                this.ModelState.AddModelError("Email", "User with this email address already exists.");
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
                    IsGoogleAccount = false
                };

                var newUser = await userService.PostAsync(userDTO);

                var user = new User
                {
                    Email = newUser.Email,
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
                this.ModelState.AddModelError("Username", ex.Message);
            }

            return View("ConfirmEmail", new EmailConfirmModel { EmailSent = true });
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
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (!await userService.IsExistingAsync(model.Credential) && !await userService.IsExistingUsernameAsync(model.Credential))
            {
                this.ModelState.AddModelError("Credential", "Incorrect combination of email/username and password.");
                return this.View(model);
            }

            try
            {
                var user = await authHelper.TryLogin(model.Credential, model.Password);

                if (user.IsEmailConfirmed == false)
                {
                    this.ModelState.AddModelError("Credential", "You have to confirm your email.");
                    return this.View(model);
                }

                if (user.IsBlocked && DateTime.Compare(DateTime.Now, user.LastBlockTime.AddDays(7)) < 0)
                {
                    DateTime unblock = user.LastBlockTime.AddDays(7);
                    TimeSpan span = (unblock - DateTime.Now);

                    this.ModelState.AddModelError("Credential", $"You were blocked on {user.LastBlockTime.ToString("dd/MM/yyyy hh:mm")}. Try again in {span.Days} days, {span.Hours} hours and {span.Minutes} minutes.");
                    return this.View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName),
                    new Claim("Image", user.ImagePath),
                };
                if (!String.IsNullOrEmpty(user.AccessToken))
                {
                    claims.Add(new Claim("AccessToken", user.AccessToken));
                }
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
                this.ModelState.AddModelError("Credential", "Incorrect combination of email/username and password.");
                return this.View(model);
            }
            
            return this.RedirectToAction("Index", "Home");
        }

        #endregion Login App

        #region Register App Modal

        [HttpGet]
        public IActionResult RegisterModal()
        {
            var register = new CreateUserViewModel();
            return this.View(register);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterModal(CreateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_RegisterPartial", model);
            }
            if (await userService.IsExistingAsync(model.Email))
            {
                return Json("User with this email address already exists.");
                //this.ModelState.AddModelError("Email", "User with this email address already exists.");
                //return this.View(model);
            }
            if (await userService.IsExistingUsernameAsync(model.Username))
            {
                return Json("User with this username already exists.");
                //this.ModelState.AddModelError("Username", "User with this username already exists.");
                //return this.View(model);
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
                    IsGoogleAccount = false
                };

                var newUser = await userService.PostAsync(userDTO);

                var user = new User
                {
                    Email = newUser.Email
                };

                await userService.GenerateEmailConfirmationTokenAsync(user);

                return RedirectToAction("ReturnConfirmEmailView", new EmailConfirmModel { EmailSent = true, Email = user.Email });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email"))
                {
                    return Json(ex.Message);
                    //this.ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    return Json(ex.Message);
                    //this.ModelState.AddModelError("Username", ex.Message);
                }
            }
        }

        #endregion Register App Modal

        #region Login App Modal

        [HttpGet]
        public IActionResult LoginModal()
        {
            var login = new LoginViewModel();
            return this.View(login);
        }

        [HttpPost]
        public async Task<IActionResult> LoginModal(LoginViewModel model)
        {
            if (!await userService.IsExistingAsync(model.Credential) && !await userService.IsExistingUsernameAsync(model.Credential))
            {
                return Json("Incorrect combination of email/username and password.");
                //return this.PartialView("_LoginPartial", model);
            }
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_LoginPartial", model);
            }

            try
            {
                var user = await authHelper.TryLogin(model.Credential, model.Password);

                if (user == null)
                {
                    return Json("You have to confirm your email.");
                    //return this.PartialView("_LoginPartial", model);
                }

                if (user.IsBlocked && DateTime.Compare(DateTime.Now, user.LastBlockTime.AddDays(7)) < 0)
                {
                    DateTime unblock = user.LastBlockTime.AddDays(7);
                    TimeSpan span = (unblock - DateTime.Now);

                    return Json($"You were blocked on {user.LastBlockTime.ToString("dd/MM/yyyy hh:mm")}. Try again in {span.Days} days, {span.Hours} hours and {span.Minutes} minutes.");
                    //return this.PartialView("_LoginPartial", model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName),
                    new Claim("Image", user.ImagePath),
                };
                if (!String.IsNullOrEmpty(user.AccessToken))
                {
                    claims.Add(new Claim("AccessToken", user.AccessToken));
                }
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
                return Json("Incorrect combination of email/username and password.");
                //return this.PartialView("_LoginPartial", model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion Login App Modal

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

        [AllowAnonymous]
        public IActionResult ReturnConfirmEmailView(EmailConfirmModel model)
        {
            return View("ConfirmEmail", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmail(EmailConfirmModel model)
        {
            try
            {

                var user = new User
                {
                    Email = model.Email
                };

                await userService.GenerateEmailConfirmationTokenAsync(user);
                return View("ConfirmEmail", new EmailConfirmModel { EmailSent = true, Email = model.Email });
            }
            catch (Exception)
            {
                return View("ConfirmEmail", new EmailConfirmModel { EmailSent = false});
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, bool resendLink = false)
        {
            try
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
                if (resendLink)
                {
                    model.EmailSent = false;
                }
                else
                {
                    model.EmailSent = true;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }

        }

        [HttpGet("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string uid, string token)
        {
            try
            {
                ResetPasswordModel resetPasswordModel = new ResetPasswordModel
                {
                    Token = token,
                    UserId = uid
                };
                return View(resetPasswordModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Token = model.Token.Replace(' ', '+');
                    var result = await userService.ResetPasswordAsync(model);
                    if (result)
                    {
                        ModelState.Clear();
                        model.IsSuccess = true;
                        return View(model);
                    }
                    ModelState.AddModelError("", "Can't change password");
                }
                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        #endregion Email actions
    }
}