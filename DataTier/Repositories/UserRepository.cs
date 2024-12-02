using DataTier.Context;
using DataTier.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
            try
            {
                if (!IsPasswordHasSpecialCharacters(newEntity.PasswordHash))
                {
                    throw new Exception();
                }
                context.Users.Add(newEntity);
                context.SaveChanges();
                return newEntity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var user = context.Users.Where(user => user.Id == id).First();
                context.Users.Remove(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User Read(Guid id)
        {
            try
            {
                var user=context.Users.Where(user => user.Id == id).First();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Update(User updatedUser)
        {
            try
            {
                var user = context.Users.Where(user => user.Id == updatedUser.Id).First();
                user=updatedUser;
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public List<User> ReadAll()
        {
            try
            {
                var users = context.Users.ToList();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsPasswordHasSpecialCharacters(string password)
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
