using MahjongBuddy.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            return await Mediator.Send(query);
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
            return await Mediator.Send(new CurrentUser.Query());
        }

        [AllowAnonymous]
        [HttpPost("facebook")]
        public async Task<ActionResult<User>> FacebookLogin(ExternalLogin.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<User>> Refresh(RefreshToken.Query query)
        {
            var principal = GetPrincipalFromExpiredToken(query.Token);
            query.UserName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return await Mediator.Send(query);
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
        [HttpPost("resendEmailVerification")]
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
        [HttpPost("resendForgotPassword")]
        public async Task<ActionResult> ResendForgotPassword([FromQuery] ResendForgotPassword.Query query)
        {
            query.Origin = Request.Headers["origin"];

            await Mediator.Send(query);

            return Ok("Password Reset link sent - please check email");
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }
    }
}
