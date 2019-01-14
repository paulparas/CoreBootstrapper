using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBootstrapper.Models;
using CoreBootstrapper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreBootstrapper.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
            {
                _logger.LogInformation("Username or password is incorrect for user {0}", userParam.Username);
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            _logger.LogInformation("Login successful for user {0}", userParam.Username);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}