using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class User
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set;} = string.Empty;

        public string Email { get; private set; } = string.Empty; 

        public string Password { get; private set; } = string.Empty;

        public string Username { get; set;} = string.Empty;

        public Address? Address { get; private set; } = default!;

        protected User() { }

        public User(string firstName, string lastName, string email, string password, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Username = username;
        }
    }
}
