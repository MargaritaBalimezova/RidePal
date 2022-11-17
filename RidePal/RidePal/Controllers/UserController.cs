using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System;
using RidePal.Web.Models;
using RidePal.WEB.Models;
using AutoMapper;
using RidePal.Data.Models;

namespace RidePal.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices userService;
        private readonly IMapper mapper;
        public IWebHostEnvironment hostingEnvironment;

        public UserController(IUserServices userService, IWebHostEnvironment hostingEnvironment, IMapper mapper)
        {
            this.userService = userService;
            this.hostingEnvironment = hostingEnvironment;
            this.mapper = mapper;
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            try
            {
                var user = await userService.GetUserDTOByEmailAsync(email);
                return View(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                return View(await userService.GetAsync());
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var user = await userService.GetUserDTOByEmailAsync(this.User.Identity.Name);

            var update = new UpdateUserViewModel
            {
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ImagePath = user.ImagePath
            };
            return this.View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var user = await userService.GetUserDTOByEmailAsync(this.User.Identity.Name);

                if (await userService.IsExistingAsync(model.Email) && model.Email != user.Email)
                {
                    this.ModelState.AddModelError("Email", "User with this email address already exists.");
                }

                var userDTO = new UpdateUserDTO();

                userDTO.Password = model.Password ?? user.Password;
                userDTO.FirstName = model.FirstName ?? user.FirstName;
                userDTO.LastName = model.LastName ?? user.LastName;
                userDTO.Email = model.Email ?? user.Email;
                if (model.File != null)
                {
                    userDTO.ImagePath = UploadPhoto(model.File);
                }

                await userService.UpdateAsync(user.Id, userDTO);
            }
            catch (Exception)
            {
                return this.View(model);
            }

            return this.RedirectToAction("Update", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                await userService.DeleteAsync(email);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        #endregion CRUD

        [HttpPost]
        public async Task<IActionResult> Search(string userSearch, int type)
        {
            try
            {
                return View("AllUsers", await userService.SearchAsync(userSearch, type));
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Block(int id)
        {
            try
            {
                await userService.BlockUserAsync(id);
                var user = await userService.GetUserDTOAsync(id);
                return RedirectToAction("Index", new { user.Email });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Unblock(int id)
        {
            try
            {
                await userService.UnblockUserAsync(id);
                var user = await userService.GetUserDTOAsync(id);
                return RedirectToAction("Index", new { user.Email });
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        #region Friends Actions

        [HttpGet]
        public async Task<IActionResult> SendFriendRequest(string senderEmail, string recipientEmail, string currentView)
        {
            try
            {
                await userService.SendFriendRequestAsync(senderEmail, recipientEmail);
                if (currentView == "allUsers")
                {
                    return RedirectToAction("AllUsers");
                }
                else if (currentView == "index")
                {
                    return RedirectToAction("Index", new { email = recipientEmail });
                }
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeclineFriendRequest(string senderEmail, string recipientEmail, string currentView)
        {
            try
            {
                await userService.DeclineFriendRequestAsync(senderEmail, recipientEmail);
                if (currentView == "friendRequests")
                {
                    return this.RedirectToAction("FriendRequests", "User", new { email = recipientEmail });
                }
                else if (currentView == "index")
                {
                    return RedirectToAction("Index", new { email = recipientEmail });
                }
                else if (currentView == "indexRemove")
                {
                    return RedirectToAction("Index", new { email = senderEmail });
                }
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AcceptFriendRequest(string senderEmail, string recipientEmail, string currentView)
        {
            try
            {
                await userService.AcceptFriendRequestAsync(senderEmail, recipientEmail);
                if (currentView == "friendRequests")
                {
                    return this.RedirectToAction("FriendRequests", "User", new { email = recipientEmail });
                }
                else if (currentView == "index")
                {
                    return RedirectToAction("Index", new { email = senderEmail });
                }
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllFriends(string email)
        {
            try
            {
                var user = await userService.GetUserDTOByEmailAsync(email);
                return View(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FriendRequests(string email)
        {
            try
            {
                var user = await userService.GetUserDTOByEmailAsync(email);
                return View(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFriend(string email, string friendEmail, string currentView)
        {
            try
            {
                await userService.RemoveFriendAsync(email, friendEmail);
                if (currentView == "allFriends")
                {
                    return RedirectToAction("AllFriends", "User", new { email });
                }
                else if (currentView == "index")
                {
                    return RedirectToAction("Index", new { email = friendEmail });
                }
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        #endregion Friends Actions

        private string UploadPhoto(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            FileInfo fi = new FileInfo(file.FileName);
            var newFileName = "Image_" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
            if (!Directory.Exists(hostingEnvironment.WebRootPath + "\\Images\\"))
            {
                Directory.CreateDirectory(hostingEnvironment.WebRootPath + "\\Images\\");
            }
            var path = Path.Combine("", hostingEnvironment.WebRootPath + "\\Images\\" + newFileName);
            using (FileStream stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
                stream.Flush();
            }
            return newFileName;
        }
    }
}