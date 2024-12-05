using DataTier.Context;
using DataTier.Entities;
using System.Text.RegularExpressions;

namespace DataTier.Repositories
{
    public class UserRepository : IEntityRepository<User>
    {
        private readonly DatabaseContext context;

        public UserRepository(DatabaseContext context) {
            this.context=context;
        }
        public User Create(User newEntity)
        {
                if (!IsPasswordHasSpecialCharacters(newEntity.PasswordHash))
                {
                    throw new Exception();
                }
                context.Users.Add(newEntity);
                context.SaveChanges();
                return newEntity;
        }

        public void Delete(Guid id)
        {
                var user = context.Users.Where(user => user.Id == id).First();
                context.Users.Remove(user);
                context.SaveChanges();
        }

        public User Read(Guid id)
        {
                var user=context.Users.Where(user => user.Id == id).First();
                return user;
        }

        public bool Update(User updatedUser)
        {
                var user = context.Users.Where(user => user.Id == updatedUser.Id).First();
                user=updatedUser;
                context.SaveChanges();
                return true;
        }

        public List<User> ReadAll()
        {
                var users = context.Users.ToList();
                return users;
        }

        private static bool IsPasswordHasSpecialCharacters(string password)
        {
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return false;
            }

            return true;
        }
    }
}
