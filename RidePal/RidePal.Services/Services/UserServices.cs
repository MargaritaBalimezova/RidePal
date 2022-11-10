using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class UserServices : IUserServices
    {
        private const string USER_NOT_FOUND = "User not found!";
        private const string INVALID_DATA = "Invalid Data!";
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public UserServices(RidePalContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            return await db.Users.Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
        }

        public async Task<UserDTO> PostAsync(UpdateUserDTO obj)
        {
            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (await IsExistingAsync(obj.Email))
            {
                throw new Exception("Email already taken");
            }

            if (await IsExistingUsernameAsync(obj.Username))
            {
                throw new Exception("Username already taken");
            }

            if (obj == null ||
                obj.FirstName.Length < 0 ||
                obj.LastName.Length < 0 ||
                obj.Password.Length < 8 ||
                obj.Username.Length < 0 ||
                !isEmailValid)
            {
                throw new Exception(INVALID_DATA);
            }

            var user = mapper.Map<User>(obj);
            user.RoleId = 2;
            user.Role = null;

            var passHasher = new PasswordHasher<User>();
            user.Password = passHasher.HashPassword(user, obj.Password);

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
                if (await IsExistingAsync(obj.Email))
                {
                    throw new Exception("Email already taken");
                }
            }

            if (obj.Password != userToUpdate.Password && obj.Password.Length < 8)
            {
                var passHasher = new PasswordHasher<User>();
                userToUpdate.Password = passHasher.HashPassword(userToUpdate, obj.Password);
            }


            if (obj.FirstName != null && obj.FirstName.Length > 0)
            {
                userToUpdate.FirstName = obj.FirstName;
            }

            if (obj.LastName != null && obj.LastName.Length > 0)
            {
                userToUpdate.LastName = obj.LastName;
            }

            if (isEmailValid)
            {
                userToUpdate.Email = obj.Email;
            }

            userToUpdate.ImagePath = obj.ImagePath ?? userToUpdate.ImagePath;


            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToUpdate);
        }

        public async Task<UserDTO> DeleteAsync(string username)
        {
            var userToDelete = await GetUserAsync(username);

            userToDelete.DeletedOn = DateTime.Now;

            db.Users.Remove(userToDelete);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToDelete);
        }

        public async Task<int> UserCount()
        {
            var numOfUsers = await GetAllUsersAsync();
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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await db.Users.Where(x => x.IsDeleted == false).ToListAsync();
            return mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(USER_NOT_FOUND);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(USER_NOT_FOUND);
        }

        public async Task SendFriendRequest(string senderUsername, string recipientUsername)
         {
            var sender = await GetUserAsync(senderUsername);
            var recipient = await GetUserAsync(recipientUsername);

            var friendRequest = new FriendRequest { SenderId = sender.Id, RecipientId = recipient.Id };

            await db.FriendRequests.AddAsync(friendRequest);

            sender.SentFriendRequests.Add(friendRequest);
            recipient.ReceivedFriendRequests.Add(friendRequest);

            await db.SaveChangesAsync();
        }

        public async Task DeclineFriendRequest(string senderUsername, string recipientUsername)
        {
            var sender = await GetUserAsync(senderUsername);
            var recipient = await GetUserAsync(recipientUsername);

            var friendRequest = new FriendRequest { SenderId = sender.Id, RecipientId = recipient.Id };

            db.FriendRequests.Remove(friendRequest);

            sender.SentFriendRequests.Remove(friendRequest);
            recipient.ReceivedFriendRequests.Remove(friendRequest);

            await db.SaveChangesAsync();
        }

        public async Task AcceptFriendRequest(string senderUsername, string recipientUsername)
        {
            var sender = await GetUserAsync(senderUsername);
            var recipient = await GetUserAsync(recipientUsername);

            var friendRequest = new FriendRequest { SenderId = sender.Id, RecipientId = recipient.Id };

            db.FriendRequests.Remove(friendRequest);

            sender.SentFriendRequests.Remove(friendRequest);
            recipient.ReceivedFriendRequests.Remove(friendRequest);

            sender.Friends.Add(recipient);
            recipient.Friends.Add(sender);

            await db.SaveChangesAsync();
        }

        public async Task RemoveFriend(string username, string friendUsername)
        {
            var user = await GetUserAsync(username);
            var friend = await GetUserAsync(friendUsername);

            user.Friends.Remove(friend);
            friend.Friends.Remove(user);

            await db.SaveChangesAsync();
        }

        public async Task BlockUser(int username)
        {
            var user = await GetUserAsync(username);

            if (user.IsBlocked == true)
            {
                throw new Exception("User is already blocked");
            }

            user.IsBlocked = true;

            await db.SaveChangesAsync();
        }

        public async Task UnblockUser(int username)
        {
            var user = await GetUserAsync(username);

            if (user.IsBlocked == false)
            {
                throw new Exception("User is not blocked");
            }

            user.IsBlocked = false;

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> Search(string userSearch, int type)
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

        public async Task<IEnumerable<UserDTO>> GetAllCommentsAsync(string username)
        {
            var user = await GetUserAsync(username);
            var friends = user.Friends.Select(x => mapper.Map<UserDTO>(x)).ToList();

            return friends ?? throw new Exception(USER_NOT_FOUND);
        }


        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync(string username)
        {
            var user = await GetUserAsync(username);
            var playlists = user.Playlists.Select(x => mapper.Map<Playlist>(x)).ToList();

            return playlists ?? throw new Exception(USER_NOT_FOUND);
        }
    }
}
