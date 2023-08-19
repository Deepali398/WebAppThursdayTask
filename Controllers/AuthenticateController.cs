using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppThursdayTask.Services;

namespace WebAppThursdayTask.Controllers
{
    /// <summary>
    ///This class has methods to authenticate the users 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController
    {
            private readonly IAuthenticate _authenticate;

            //Dependency Injection
            public AuthenticateController(IAuthenticate authenticate)
            {
                _authenticate = authenticate;
            }

            // This method is used to authenticate a user
            [HttpPatch("authenticate")]
            public async Task<ActionResult<string>> AuthenticateUserAsync(string userName, string password)
            {
                var usertok = await _authenticate.AuthenticateUserAsync(userName, password);
                return usertok;
            }
        }
}

