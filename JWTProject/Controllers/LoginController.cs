using JWTProject.Context;
using JWTProject.JWTAuthentication;
using JWTProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly OkdbContext _okdbContext;
        readonly IConfiguration _configuration;

        public LoginController(OkdbContext okdbContext, IConfiguration configuration)
        {
            _okdbContext = okdbContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Token> Login(User user)
        {
            TokenUtils tokenUtils = new TokenUtils(_configuration);

            var userInfo = await _okdbContext.Users.FirstOrDefaultAsync(x => x.Mail == user.Mail && x.Password == user.Password);

            if (userInfo != null)
            {
                var token = tokenUtils.GenerateJWTToken(userInfo);
                return token;
            }

            return null;
        }

        [Authorize]
        [HttpGet("getUserList")]
        public async Task<List<string?>> GetUserList()
        {
            var users = await _okdbContext.Users
                 .Select(x => x.UserName).ToListAsync();

            return users;
        }

    }
}
