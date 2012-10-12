namespace RedisMembershipProvider.Core
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        void Save(User user);
        User GetById(int userId);
    }
}