using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Web.Helpers;
using RidePal.Web.Models;
using RidePal.WEB.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserServices userService;
        private readonly IAuthHelper authHelper;

        public IWebHostEnvironment hostingEnvironment;

        public UserApiController(IUserServices userService, IAuthHelper authHelper, IWebHostEnvironment hostEnvironment)
        {
            this.userService = userService;
            this.authHelper = authHelper;
            this.hostingEnvironment = hostEnvironment;
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userService.GetAsync();
                return this.Ok(users);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await userService.GetUserDTOAsync(id);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel user)
        {
            try
            {
                var userDTO = new UpdateUserDTO
                {
                    Password = user.Password,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                var newUser = await userService.PostAsync(userDTO);
                return this.Ok(newUser);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var user = await userService.DeleteAsync(email);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUserViewModel user)
        {
            try
            {
                var path = UploadPhoto(user.File);

                var userDTO = new UpdateUserDTO
                {
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                if (path != null)
                {
                    userDTO.ImagePath = path;
                }

                var newUser = await userService.UpdateAsync(id, userDTO);
                return this.Ok(newUser);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #endregion CRUD

        #region Friends

        [HttpGet]
        [Route("user/{email}/friends")]
        public async Task<IActionResult> GetAllFriendsAsync(string email)
        {
            try
            {
                var friends = await userService.GetAllFriendsAsync(email);
                return this.Ok(friends);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{email}/friendsrequests")]
        public async Task<IActionResult> GetAllFriendRequests(string email)
        {
            try
            {
                var friendRequests = await userService.GetAllFriendRequestsAsync(email);
                return this.Ok(friendRequests);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("user/{senderEmail}/sendfriendrequest/{recipientEmail}")]
        public async Task<IActionResult> SendFriendRequest(string senderEmail, string recipientEmail)
        {
            try
            {
                await userService.SendFriendRequestAsync(senderEmail, recipientEmail);
                return this.Ok("Request sent successfully!");
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("user/{senderEmail}/acceptfriendrequest/{recipientEmail}")]
        public async Task<IActionResult> AcceptFriendRequest(string senderEmail, string recipientEmail)
        {
            try
            {
                await userService.AcceptFriendRequestAsync(senderEmail, recipientEmail);
                return this.Ok("Request accepted successfully!");
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("user/{senderEmail}/declinefriendrequest/{recipientEmail}")]
        public async Task<IActionResult> DeclineFriendRequest(string senderEmail, string recipientEmail)
        {
            try
            {
                await userService.DeclineFriendRequestAsync(senderEmail, recipientEmail);
                return this.Ok("Request declined successfully!");
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("user/{email}/removefriend/{friendEmail}")]
        public async Task<IActionResult> RemoveFriend(string email, string friendEmail)
        {
            try
            {
                await userService.RemoveFriendAsync(email, friendEmail);
                return this.Ok("Friend removed successfully!");
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        #endregion Friends

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel userModel)
        {
            try
            {
                var user = await authHelper.TryLogin(userModel.Credential, userModel.Password);
                return this.Ok("Logged in successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        //Admin
        [HttpPut]
        [Route("block/{email}")]
        public async Task<IActionResult> Block(string email)
        {
            try
            {
                await userService.BlockUserAsync(email);
                return this.Ok("User blocked successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("unblock/{email}")]
        public async Task<IActionResult> Unblock(string email)
        {
            try
            {
                await userService.UnblockUserAsync(email);
                return this.Ok("User unblocked successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{email}/playlists")]
        public async Task<IActionResult> GetAllPlaylistsAsync(string email)
        {
            try
            {
                var playlists = await userService.GetAllPlaylistsAsync(email);
                return this.Ok(playlists);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

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
            return path;
        }
    }
}