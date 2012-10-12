using System;

namespace RedisMembershipProvider.Core
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Id = -1;
        }

        public string Username { get; private set; }
        public string Password { get; set; }

        public int Id { get; private set; }
        public string ConfirmationToken { get; private set; }

        internal void MarkPersistent(int id)
        {
            Id = id;
            ConfirmationToken = Guid.NewGuid().ToString();
        }

        public bool IsPersistent
        {
            get { return Id != -1; }
        }
    }

    public class UserNameLookup
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public string Id { get { return Username; } }
    }
}