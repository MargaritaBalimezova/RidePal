using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public UserServices(IEmailService emailService, IConfiguration configuration, RidePalContext context, IMapper mapper)
        {
            this.emailService = emailService;
            this.configuration = configuration;
            this.db = context;
            this.mapper = mapper;
        }

        #region CRUD operations

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            return await db.Users.Where(x => x.IsDeleted == false).Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
        }

        public async Task<UserDTO> PostAsync(UpdateUserDTO obj)
        {
            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (await IsExistingForRegistrationAsync(obj.Email))
            {
                throw new Exception(string.Format(Constants.ALREADY_TAKEN, "Email"));
            }

            if (await IsExistingUsernameForRegistrationAsync(obj.Username))
            {
                throw new Exception(string.Format(Constants.ALREADY_TAKEN, "Username"));
            }

            if (obj == null ||
                obj.FirstName.Length < Constants.USER_FIRSTNAME_MIN_LENGTH ||
                obj.LastName.Length < Constants.USER_LASTNAME_MIN_LENGTH ||
                obj.Username.Length < Constants.USER_USERNAME_MIN_LENGTH ||
                !isEmailValid)
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            var user = mapper.Map<User>(obj);
            user.RoleId = 2;

            if (!String.IsNullOrEmpty(obj.Password))
            {
                var passHasher = new PasswordHasher<User>();
                user.Password = passHasher.HashPassword(user, obj.Password);
            }
            user.ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg";
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(obj);
        }

        public async Task<UserDTO> UpdateAsync(int id, UpdateUserDTO obj)
        {
            var userToUpdate = await GetUserAsync(id);

            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (obj.Email != userToUpdate.Email)
            {
                if (await IsExistingForRegistrationAsync(obj.Email))
                {
                    throw new Exception(string.Format(Constants.ALREADY_TAKEN, "Email"));
                }
            }

            if (obj.Password != userToUpdate.Password && obj.Password.Length >= Constants.USER_PASSWORD_MIN_LENGTH)
            {
                var passHasher = new PasswordHasher<User>();
                userToUpdate.Password = passHasher.HashPassword(userToUpdate, obj.Password);
            }

            if (obj.FirstName != userToUpdate.FirstName && obj.FirstName.Length >= Constants.USER_FIRSTNAME_MIN_LENGTH)
            {
                userToUpdate.FirstName = obj.FirstName;
            }

            if (obj.LastName != userToUpdate.LastName && obj.LastName.Length >= Constants.USER_LASTNAME_MIN_LENGTH)
            {
                userToUpdate.LastName = obj.LastName;
            }

            if (!String.IsNullOrEmpty(obj.AccessToken) && userToUpdate.AccessToken != obj.AccessToken)
            {
                userToUpdate.AccessToken = obj.AccessToken;
            }

            userToUpdate.ImagePath = obj.ImagePath ?? userToUpdate.ImagePath;

            if (isEmailValid && userToUpdate.Email != obj.Email)
            {
                userToUpdate.Email = obj.Email;
                userToUpdate.IsEmailConfirmed = false;
                await GenerateEmailConfirmationTokenAsync(userToUpdate);
            }

            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToUpdate);
        }

        public async Task<UserDTO> DeleteAsync(string email)
        {
            var userToDelete = await GetUserByEmailAsync(email);

            userToDelete.DeletedOn = DateTime.Now;

            await db.FriendRequests.Where(x => x.SenderId == userToDelete.Id).ForEachAsync(x => x.IsDeleted = true);

            await db.Users.ForEachAsync(x => x.ReceivedFriendRequests.Where(x => x.SenderId == userToDelete.Id).ToList().ForEach(x => x.IsDeleted = true));

            db.Users.Remove(userToDelete);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToDelete);
        }

        #endregion CRUD operations

        public async Task<int> UserCount()
        {
            var numOfUsers = await GetAsync();
            return numOfUsers.Count();
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email && x.IsDeleted == false);
        }

        public async Task<bool> IsExistingUsernameAsync(string username)
        {
            return await db.Users.AnyAsync(x => x.Username == username && x.IsDeleted == false);
        }

        private async Task<bool> IsExistingForRegistrationAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> IsExistingUsernameForRegistrationAsync(string username)
        {
            return await db.Users.AnyAsync(x => x.Username == username);
        }

        #region GetUser methods

        private async Task<User> GetUserAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        private async Task<User> GetUserAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOByEmailAsync(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception();
        }

        public async Task<UserDTO> GetUserDTOAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        #endregion GetUser methods

        #region Friends methods

        public async Task SendFriendRequestAsync(string senderUsername, string recipientUsername)
        {
            if (senderUsername != recipientUsername)
            {
                var sender = await GetUserByEmailAsync(senderUsername);
                var recipient = await GetUserByEmailAsync(recipientUsername);

                var friendRequest = new FriendRequest { SenderId = sender.Id, RecipientId = recipient.Id };

                if (!db.FriendRequests.Any(x => x.Recipient.Email == recipient.Email && x.Sender.Email == sender.Email && x.IsDeleted == false))
                {
                    sender.SentFriendRequests.Add(friendRequest);
                    recipient.ReceivedFriendRequests.Add(friendRequest);

                    await db.FriendRequests.AddAsync(friendRequest);

                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeclineFriendRequestAsync(string senderEmail, string recipientEmail)
        {
            var sender = await GetUserByEmailAsync(senderEmail);
            var recipient = await GetUserByEmailAsync(recipientEmail);

            var friendRequest = await db.FriendRequests.FirstOrDefaultAsync(x => x.Sender.Email == senderEmail && x.Recipient.Email == recipientEmail);

            sender.SentFriendRequests.Remove(friendRequest);
            recipient.ReceivedFriendRequests.Remove(friendRequest);

            friendRequest.IsDeleted = true;
            friendRequest.DeletedOn = DateTime.Now;
            //////TODO Resolve -cascade delete?
            //db.FriendRequests.Remove(friendRequest);

            await db.SaveChangesAsync();
        }

        public async Task AcceptFriendRequestAsync(string senderEmail, string recipientEmail)
        {
            var sender = await GetUserByEmailAsync(senderEmail);
            var recipient = await GetUserByEmailAsync(recipientEmail);

            var friendRequest = await db.FriendRequests.FirstOrDefaultAsync(x => x.Sender.Email == senderEmail && x.Recipient.Email == recipientEmail);

            sender.SentFriendRequests.Remove(friendRequest);
            recipient.ReceivedFriendRequests.Remove(friendRequest);

            sender.Friends.Add(recipient);
            recipient.Friends.Add(sender);

            friendRequest.IsDeleted = true;
            friendRequest.DeletedOn = DateTime.Now;
            ////TODO Resolve -cascade delete?
            //db.FriendRequests.Remove(friendRequest);

            await db.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(string email, string friendEmail)
        {
            var user = await GetUserByEmailAsync(email);
            var friend = await GetUserByEmailAsync(friendEmail);

            user.Friends.Remove(friend);
            friend.Friends.Remove(user);

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllFriendsAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var friends = user.Friends.Where(x => x.IsDeleted == false).Select(x => mapper.Map<UserDTO>(x)).ToList();

            return friends ?? throw new Exception("No friends found!");
        }

        public async Task<IEnumerable<FriendRequest>> GetAllFriendRequestsAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            var friends = user.ReceivedFriendRequests.Where(x => x.IsDeleted == false).ToList();

            return friends ?? throw new Exception("Fr requests not found!");
        }

        #endregion Friends methods

        public async Task BlockUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user.NumOfBlocks >= 3)
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.Now;
            }
            else
            {
                if (user.IsBlocked == true)
                {
                    throw new Exception("User is already blocked");
                }
                user.IsBlocked = true;
                user.NumOfBlocks++;
                user.LastBlockTime = DateTime.Now;
            }
            await db.SaveChangesAsync();
        }

        public async Task UnblockUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user.IsBlocked == false)
            {
                throw new Exception("User is unblocked");
            }

            user.IsBlocked = false;

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> SearchAsync(string userSearch, int type)
        {
            IEnumerable<UserDTO> userQuery = null;

            if (String.IsNullOrEmpty(userSearch))
            {
                return userQuery = await db.Users.Where(x => x.IsDeleted == false).Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
            }

            switch (type)
            {
                case 1:
                    userQuery = await db.Users.Where(x => (x.Username.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;

                case 2:
                    userQuery = await db.Users.Where(x => (x.Email.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;

                case 3:
                    userQuery = await db.Users.Where(x => (x.FirstName.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;
            }
            return userQuery;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAllPlaylistsAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var playlists = user.Playlists.Select(x => mapper.Map<PlaylistDTO>(x)).ToList();

            return playlists ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        #region Email methods

        public async Task GenerateForgotPasswordTokenAsync(User user)
        {
            var token = Guid.NewGuid().ToString("d").Substring(1, 8);

            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmailAsync(user, token);
            }
        }

        private async Task SendForgotPasswordEmailAsync(User user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions

            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForForgotPassword(options);
        }

        public async Task<bool> ResetPasswordAsyncAsync(ResetPasswordModel model)
        {
            var user = await GetUserAsync(int.Parse(model.UserId));
            if (user != null)
            {
                var passHasher = new PasswordHasher<User>();
                user.Password = passHasher.HashPassword(user, model.NewPassword);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            if (user.Id == 0)
            {
                user = await GetUserAsync(user.Username);
            }
            var token = Guid.NewGuid().ToString("d").Substring(1, 8);

            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmailAsync(user, token);
            }
        }

        private async Task SendEmailConfirmationEmailAsync(User user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForEmailConfirmation(options);
        }

        public async Task<bool> ConfirmEmailAsync(string uid, string token)
        {
            var user = await GetUserAsync(int.Parse(uid));
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion Email methods
    }
}