using System;
using System.Collections.Generic;
using System.Linq;
using TaskDB = DataTier.Entities.Task;

namespace ApplicationTier.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<TaskDB>? Tasks { get; set; }

        public User(Guid id,string username,string email,string password,List<TaskDB> tasks)
        {
            this.Id = id;
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.Tasks = tasks;
        }
    }
}
