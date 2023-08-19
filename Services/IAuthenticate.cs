using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppThursdayTask.Services
{

    public interface IAuthenticate
    {
        Task<ActionResult<string>> AuthenticateUserAsync(string userName, string password);

    }
}

