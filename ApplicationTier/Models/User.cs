using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTier.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public User(Guid id,string username,string email,string password)
        {
            this.Id = id;
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }
    }
}
