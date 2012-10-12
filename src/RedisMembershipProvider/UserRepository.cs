using System;
using RedisMembershipProvider.Core;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace RedisMembershipProvider.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IRedisClient _client;

        private readonly IRedisTypedClient<User> _typedUserClient;
        private readonly IRedisTypedClient<UserNameLookup> _typedUserNameLookupClient;

        public UserRepository(string redisUrl)
        {
            _client = new RedisClient(new Uri(redisUrl));
            _typedUserClient = _client.GetTypedClient<User>();
            _typedUserNameLookupClient = _client.GetTypedClient<UserNameLookup>();
        }

        public User GetByUsername(string username)
        {
            var lookup = _typedUserNameLookupClient.GetById(username);
            var user = _typedUserClient.GetById(lookup.UserId);
            return user;
        }

        public void Save(User user)
        {
            if (!user.IsPersistent)
            {
                user.MarkPersistent(Convert.ToInt32(_typedUserClient.GetNextSequence()));
                _typedUserClient.Store(user);
                _typedUserNameLookupClient.Store(new UserNameLookup {UserId = user.Id, Username = user.Username});
            }
            else
            {
                _typedUserClient.Store(user);
            }
        }

        public User GetById(int userId)
        {
            return _typedUserClient.GetById(userId);
        }
    }
}