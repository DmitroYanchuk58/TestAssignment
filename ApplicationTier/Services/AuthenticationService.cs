using ApplicationTier.Interfaces;
using DataTier.Repositories;
using ApplicationTier.Models;
using db = DataTier.Entities;

namespace ApplicationTier.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserRepository _repository;

        public AuthenticationService(UserRepository repository)
        {
            this._repository = repository;
        }
        public bool Authenticate(string username, string password)
        {
            var user=FindUser(username);
            if (user != null&& VerifyPassword(password, user.Password))
            {
                 return true;
            }
            return false;
        }

        public void Register(User user)
        {
            if (IsUserExist(user.Username, user.Email))
                throw new InvalidOperationException("User already exists.");

            var dbUser = ConvertFromUserToDbUser(user);

            try
            {
                _repository.Create(dbUser);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private db.User ConvertFromUserToDbUser(User user)
        {
            var convertedUser = new db.User()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                CreatedAt = DateTime.UtcNow
            };
            return convertedUser;
        }

        private User ConvertFromDbUserToUser(db.User user)
        {
            var convertedUser = new User(user.Id,user.Username,user.Email,user.PasswordHash,user.Tasks);
            return convertedUser;
        }

        private User FindUser(string username)
        {
            try
            {
                var dbUsers = _repository.ReadAll();
                var dbFoundUsers = dbUsers.Where(user => user.Username == username);
                if (!dbFoundUsers.Any())
                {
                    return null;
                }
                var dbUser = dbFoundUsers.First();
                var user = ConvertFromDbUserToUser(dbUser);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private User FindUser(string username,string email)
        {
            try
            {
                var dbUsers = _repository.ReadAll();
                var dbFoundUsers = dbUsers.Where(user => user.Username == username && user.Email == email);
                if (!dbFoundUsers.Any())
                {
                    return null;
                }
                var dbUser = dbFoundUsers.First();
                var user = ConvertFromDbUserToUser(dbUser);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsUserExist(string username,string email)
        {
            var isUserExists = FindUser(username, email) != null;
            return isUserExists;
        }
    }
}
