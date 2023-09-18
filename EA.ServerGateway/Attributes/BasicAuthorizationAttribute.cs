using Microsoft.AspNetCore.Authorization;

namespace EA.ServerGateway.Attributes;

public class BasicAuthorizationAttribute : AuthorizeAttribute
{
    public BasicAuthorizationAttribute()
    {
        Policy = "BasicAuthentication";
    }
}