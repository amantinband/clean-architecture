using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Security.ApiKeyValidation;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string API_KEY_HEADER_NAME = "X-API-KEY";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var apiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var configuredApiKey = Context.RequestServices.GetRequiredService<IConfiguration>()["ApiKeySettings:Secret"];
        if (string.IsNullOrEmpty(configuredApiKey) || configuredApiKey != apiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Api Key is invalid."));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "API-User") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));

        throw new NotImplementedException();
    }
}
