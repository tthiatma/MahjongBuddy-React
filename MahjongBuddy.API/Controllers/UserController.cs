using MahjongBuddy.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MahjongBuddy.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            var user = await Mediator.Send(query);
            SetTokenCookie(user.RefreshToken);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(Register.Command command)
        {
            command.Origin = Request.Headers["origin"];
            await Mediator.Send(command);
            return Ok("Registration successful - please check your email");
        }

        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            var user = await Mediator.Send(new CurrentUser.Query());
            SetTokenCookie(user.RefreshToken);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("facebook")]
        public async Task<ActionResult<User>> FacebookLogin(ExternalLogin.Query query)
        {
            var user = await Mediator.Send(query);
            SetTokenCookie(user.RefreshToken);
            return user;
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<User>> RefreshToken(RefreshToken.Command command)
        {
            command.RefreshToken = Request.Cookies["refreshToken"];
            var user = await Mediator.Send(command);
            SetTokenCookie(user.RefreshToken);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("verifyEmail")]
        public async Task<ActionResult> VerifyEmail(ConfirmEmail.Command command)
        {
            var result = await Mediator.Send(command);
            if (!result.Succeeded) return BadRequest("Problem verifying email address");
            return Ok("Email confirmed - you can now login");
        }

        [AllowAnonymous]
        [HttpGet("resendEmailVerification")]
        public async Task<ActionResult> ResendEmailVerification([FromQuery] ResendEmailVerification.Query query)
        {
            query.Origin = Request.Headers["origin"];

            await Mediator.Send(query);

            return Ok("Email verification link sent - please check email");
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPassword.Command command)
        {
            command.Origin = Request.Headers["origin"];
            await Mediator.Send(command);
            return Ok("Password reset link has been sent if the account exists - please check your email");
        }

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPassword.Command command)
        {
            var result = await Mediator.Send(command);
            if (!result.Succeeded) return BadRequest("Problem resetting password");
            return Ok("Password is reset- please login with your new passowrd");
        }

        [AllowAnonymous]
        [HttpGet("resendForgotPasswordVerification")]
        public async Task<ActionResult> ResendForgotPassword([FromQuery] ResendForgotPassword.Query query)
        {
            query.Origin = Request.Headers["origin"];

            await Mediator.Send(query);

            return Ok("Password Reset link sent - please check email");
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
