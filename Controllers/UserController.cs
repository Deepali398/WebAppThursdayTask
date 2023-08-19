using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppThursdayTask.Models;
using WebAppThursdayTask.Services;

namespace WebAppThursdayTask.Controllers
{
    /// <summary>
	/// This class is used as a User entity controller
	/// </summary>
	[Route("api/[controller]")]
    
    [ApiController]
    public class UsersController
    {
        private readonly IUser _users;
        public UsersController(IUser users)
        {
            _users = users;
        }

        [Authorize]
        //Http delete method implementation
        [HttpDelete]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            var action = await _users.DeleteUserAsync(id);
            return (action);
        }


        //Http Post method implementation
        [HttpPost]
        public async Task<ActionResult> RegisterUserAsync(UserView userView)
        {
            var action = await _users.RegisterUserAsync(userView);
            return (action);

        }

        [Authorize]
        //Http Put method implementation
        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(UserView userView)
        {

            var action = await _users.UpdateUserAsync(userView);
            return (action);

        }

        //Http patch method used to implement searching
        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<User>> AuthenticateUserAsync(string userName, string password)
        {
            var user = await _users.AuthenticateUserAsync(userName, password);
            return user;
        }
    }
}
