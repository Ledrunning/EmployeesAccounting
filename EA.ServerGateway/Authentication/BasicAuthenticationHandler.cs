using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using EA.Repository;
using EA.ServerGateway.Configuration;
using EA.ServerGateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EA.ServerGateway.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly EaConfiguration _configuration;
    private readonly IOptionsMonitor<AuthenticationSchemeOptions> _options;
    private IConfiguration _appConfiguration;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder, 
        ISystemClock clock, 
        IOptions<EaConfiguration> configuration,
        IConfiguration appConfiguration) : base(options, logger, encoder, clock)
    {
        _options = options;
        _configuration = configuration.Value;
        _appConfiguration = appConfiguration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Authorization header missing.");
        }

        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var authHeaderRegex = new Regex(@"Basic (.*)");

        if (!authHeaderRegex.IsMatch(authorizationHeader))
        {
            return AuthenticateResult.Fail("Authorization code not formatted properly.");
        }

        var authBase64 =
            Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
        var authUsername = authSplit[0];
        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

        var connectionString = _appConfiguration.GetConnectionString("DbConnection");
        // Manually create an instance of DatabaseContext
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(connectionString); // Use your actual connection string

        await using var dbContext = new DatabaseContext(optionsBuilder.Options);

        // Retrieve the hashed password from the database based on the username
        if (dbContext.Administrators != null)
        {
            var administrator = await dbContext.Administrators.FirstOrDefaultAsync(u => u.Login == authUsername);

            if (administrator == null || !BCrypt.Net.BCrypt.Verify(authPassword, administrator.Password))
            {
                return AuthenticateResult.Fail("Invalid Username or Password.");
            }
        }

        var authenticatedUser =
            new AuthenticatedUser("BasicAuthentication", true, authUsername);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}