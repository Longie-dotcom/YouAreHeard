using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool EmailExists(string email);
        bool IsEmailVerified(string email);
        bool PhoneExists(string phone);
        bool InsertUser(UserDTO user);
        UserDTO GetUserByEmail(string email);
        UserDTO GetUserById(int id);
    }
}