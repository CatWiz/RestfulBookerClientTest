using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace RestfulBookerAPIClient.Auth;

public class RestfulBookerAuthenticationProvider : IAuthenticationProvider
{
    private readonly IAccessTokenProvider _accessTokenProvider;

    public RestfulBookerAuthenticationProvider(IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        additionalAuthenticationContext ??= new Dictionary<string, object>();
        additionalAuthenticationContext["method"] = request.HttpMethod;
        
        var token = _accessTokenProvider.GetAuthorizationTokenAsync(request.URI, additionalAuthenticationContext, cancellationToken).Result;
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Cookie", $"token={token}");
        }
        return Task.CompletedTask;
    }
}