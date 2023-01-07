using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ChatZorro.Entities;
using ChatZorro.Services;

namespace ChatZorro.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        static Models.AuthenticateModel Authentication;
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            Authentication = new Models.AuthenticateModel();
        }

        internal static Models.AuthenticateModel GetAuthorizationSession()
        {

            return Authentication;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            Profile profile = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                Authentication.Username = credentials[0];
                Authentication.Password = credentials[1];
                profile = await _userService.Authenticate(Authentication.Username, Authentication.Password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (profile.User == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, profile.User.Code.ToString()),
                new Claim(ClaimTypes.Name, profile.User.Username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}