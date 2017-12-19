using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moni.Authorization;
using Moni.Enums;
using Moni.Extensions;
using Moni.Repository;
using Moni.ViewModels;

namespace Moni.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post(
                [FromBody] AuthViewModel auth,
                [FromServices] SigningConfigurations signingConfigurations,
                [FromServices] TokenConfigurations tokenConfigurations,
                [FromServices] MoniContext context)
        {
            var userRepo = new UserRepository(context);

            var loginStatus = userRepo.Authorize(auth);

            if (loginStatus == AuthStatus.Success)
            {
                return Ok(GenerateJwtToken(auth, signingConfigurations, tokenConfigurations));
            }

            return Forbid(loginStatus.GetDescription());
        }

        private static dynamic GenerateJwtToken(AuthViewModel auth, SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(auth.Username, "Username"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, auth.Username)
                }
            );

            var creation = DateTime.Now;
            var expiration = creation + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = creation,
                Expires = expiration
            });
            var token = handler.WriteToken(securityToken);

            return new
            {
                token,
                created = creation.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expiration.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}