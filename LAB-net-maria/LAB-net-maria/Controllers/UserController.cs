using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Logical;
using Lab.Application.Models.DTOs.WebSocket;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LAB_net_maria.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]  
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebSocketService _webSocketService;
        private readonly IUserAttributesService _userAttributesService;

        public UserController(IUserService userService, IWebSocketService webSocketService, IUserAttributesService userAttributesService)
        {
            _userService = userService;
            _userAttributesService = userAttributesService;
            _webSocketService = webSocketService;
        }

        [HttpPost("updateUserStatus")]
        public async Task<IActionResult> Login([FromBody] UserStatusDTO userStatus)
        {
            try
            {
                _userAttributesService.UserStatusUpdated += (s, e) => { _webSocketService.NotifySubscribers(AppNoificationEvents.UserStatusUpdated, e); };
                await _userAttributesService.UpdateStatusAsync(userStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("/authadmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users + "This should be available only if you have admin role!");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
    }
}
