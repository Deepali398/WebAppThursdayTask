using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppThursdayTask.Data;
using WebAppThursdayTask.Models;
using WebAppThursdayTask.Services;

namespace WebAppThursdayTask.Repositories
{
    public class AuthenticateRepo : ControllerBase, IAuthenticate
    {
        private readonly IUser _user;
        private readonly IConfiguration _configuration;
        private readonly DataItemDbContext _dataItemDbContext;
        public AuthenticateRepo(IConfiguration configuration, IUser user, DataItemDbContext dataItemDbContext)
        {
            _configuration = configuration;
            _user = user;
            this._dataItemDbContext = dataItemDbContext ?? throw new ArgumentNullException(nameof(_dataItemDbContext));
        }

        public async Task<ActionResult<string>> AuthenticateUserAsync(string userName, string password)
        {

            try
            {
                //validate username and password
                userName = userName.Trim();
                password = password.Trim();
                if (userName == null || password == null)
                {
                    return BadRequest("User name or password can't be null or empty");
                }


                List<User> user = await _dataItemDbContext.Users.Where(user => (user.UserName == userName && user.Password == password)).ToListAsync();

                if (user.Any())
                {
                    //create a Token
                    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("kjdkjsddfnsnsdnfsnsdlfjsdfsdlnfsdnndsn"));
                    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    //The Claims

                    string id = user.FirstOrDefault().UserId.ToString();
                    string name = user.FirstOrDefault().UserName.ToString();
                           
                        var claimsForToken = new List<Claim>
                        {
                            new Claim("ID", id ),
                            new Claim("Name", name)                    
                        };

                    //Create token              
                    var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication : Issuer"],
                                                                _configuration["Authentication : Audience"],
                                                                claimsForToken,
                                                                DateTime.UtcNow,
                                                                DateTime.UtcNow.AddHours(1),
                                                                signingCredentials);
                    var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                    return Ok(tokenToReturn);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Server error");
            }
        }

    }
}

