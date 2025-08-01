using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IUserService
    {

        Task<IActionResult> RegisterAsync(UserDTO newUser);
        Task<IActionResult> LoginAsync(LoginDTO loginDTO);
        Task<IActionResult> LogoutAsync();
        UserDTO GetUserById(int id);
        void SaveFacebookPSID(int userId, string senderId);
    }
}