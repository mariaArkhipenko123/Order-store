using Lab.Application.Extensions;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Logical;
using Lab.Application.Models.DTOs.Secure;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LAB_net_maria.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebSocketService _webSocketService;
        private readonly IUserAttributesService _userAttributesService;

        public AuthController(IUserService userService, IWebSocketService webSocketService, IUserAttributesService userAttributesService)
        {
            _userService = userService;
            _webSocketService = webSocketService;
            _userAttributesService = userAttributesService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] АuthorDTO authParams)
        {
            if (authParams == null || string.IsNullOrEmpty(authParams.Email) || string.IsNullOrEmpty(authParams.Password))
                return BadRequest("Invalid client request");

            var regResult = await _userService.RegisterAsync(authParams);

            if (!regResult.Succeeded)
                return StatusCode(500, regResult.Errors);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] АuthorDTO authParams)
        {
            if (authParams == null || string.IsNullOrEmpty(authParams.Email) || string.IsNullOrEmpty(authParams.Password))
                return BadRequest("Invalid client request");

            var (Result, Token, RefreshToken) = await _userService.LoginAsync(authParams);

            if (!Result.Succeeded)
                return Unauthorized(Result.Errors);

            return Ok(new
            {
                Token,
                RefreshToken
            });
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginDTO googleLoginParams)
        {
            if (googleLoginParams == null || string.IsNullOrEmpty(googleLoginParams.IdToken))
                return BadRequest("Invalid client request");

            var (result, token, refreshToken) = await _userService.LoginWithGoogle(googleLoginParams);

            if (!result.Succeeded)
                return Unauthorized(result.Errors);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("login/firebase")]
        public async Task<IActionResult> LoginWithFirebase([FromBody] FirebaseLoginDTO firebaseLoginParams)
        {
            if (firebaseLoginParams == null || string.IsNullOrEmpty(firebaseLoginParams.IdFirebaseToken))
                return BadRequest("Invalid client request");

            var (result, token, refreshToken) = await _userService.LoginWithFirebase(firebaseLoginParams);

            if (!result.Succeeded)
                return Unauthorized(result.Errors);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }
        [HttpGet("statusUpdates")]
        public async Task StatusUpdateWebSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await webSocket.SendJsonAsync(new { qwe = "init message" });
                _webSocketService.AddSubscriber(AppNoificationEvents.UserStatusUpdated, webSocket);
                await webSocket.KeepAliveAndDiscardRecieve();
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
