using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppThursdayTask.Data;
using WebAppThursdayTask.Models;
using WebAppThursdayTask.Services;

namespace WebAppThursdayTask.Repositories
{
    public class UserRepo:ControllerBase,IUser 
    {
        private readonly DataItemDbContext _dataItemDbContext;
        //Dependency Injection
        public UserRepo(DataItemDbContext dataItemDbContext)
        {
            this._dataItemDbContext = dataItemDbContext ?? throw new ArgumentNullException(nameof(_dataItemDbContext));
        }

        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                User user = _dataItemDbContext.Users.FirstOrDefault(x=>x.UserId==id) ?? throw new ArgumentNullException(nameof(user));
                if (user == null)
                {
                    return NotFound($"User with id : {id} not found");
                }
                else
                {
                    _dataItemDbContext.Users.Remove(user);
                    await _dataItemDbContext.SaveChangesAsync(true);
                    return Ok("User Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult<User>> AuthenticateUserAsync(string userName, string password)
        {
            try
            {
                userName = userName.Trim();
                password = password.Trim();
                if (userName == null || password == null)
                {
                    return BadRequest("User name or password can't be null or empty");
                }


                var user = await _dataItemDbContext.Users.Where(user => (user.UserName == userName && user.Password == password))
                                                       .ToListAsync();
                if (user.Any())
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("User matching the user name and password not found");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }

        public async Task<ActionResult> RegisterUserAsync(UserView userView)
        {
            try
            { User user = new();
                user.UserName = userView.UserName;
                user.Password = userView.Password;
                user.Email = userView.Email;

                await _dataItemDbContext.Users.AddAsync(user);
                await _dataItemDbContext.SaveChangesAsync();
                return Ok("User Created.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> UpdateUserAsync(UserView userView)
        {
            try
            {
                if (userView == null) 
                {
                   return BadRequest("User not found . Data sent is invalid.");
                }
                var user1 = await _dataItemDbContext.Users.FirstOrDefaultAsync(u=>u.UserName==userView.UserName);

                if (user1 == null)
                {
                    return BadRequest("User Not Found");
                }

                user1.UserName = userView.UserName;
                user1.Password = userView.Password;
                user1.Email = userView.Email;


                await _dataItemDbContext.SaveChangesAsync();
                return Ok("User Details Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
