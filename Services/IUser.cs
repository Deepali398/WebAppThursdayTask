using Microsoft.AspNetCore.Mvc;
using WebAppThursdayTask.Models;

namespace WebAppThursdayTask.Services
{
    public interface IUser
    {
        Task<ActionResult<User>> AuthenticateUserAsync(string userName, string password);
        Task<ActionResult> DeleteUserAsync(int id);
        Task<ActionResult> RegisterUserAsync(UserView userView);
        Task<ActionResult> UpdateUserAsync(UserView userView);
    }
}
