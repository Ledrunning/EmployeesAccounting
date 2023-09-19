using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using EA.ServerGateway.Configuration;
using EA.ServerGateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace EA.ServerGateway.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly EaConfiguration _configuration;
    private readonly IOptionsMonitor<AuthenticationSchemeOptions> _options;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IOptions<EaConfiguration> configuration) : base(options, logger,
        encoder, clock)
    {
        _options = options;
        _configuration = configuration.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
        }

        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var authHeaderRegex = new Regex(@"Basic (.*)");

        if (!authHeaderRegex.IsMatch(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
        }

        var authBase64 =
            Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
        var authUsername = authSplit[0];
        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

        if (authUsername != _configuration.Login ||
            authPassword != _configuration.Password)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password."));
        }

        var authenticatedUser =
            new AuthenticatedUser("BasicAuthentication", true, _configuration.Login);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
    }
}