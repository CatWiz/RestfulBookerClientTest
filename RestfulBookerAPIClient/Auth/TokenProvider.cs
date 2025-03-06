using System.Net.Http.Json;
using System.Text.Json;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace ReqresAPITest.Auth;

public class RestfulBookerAccessTokenProvider : IAccessTokenProvider
{
    private readonly string _baseUrl;
    private readonly AuthParams _authParams;
    private readonly HttpClient _httpClient;
    private string? _token;
    
    public AllowedHostsValidator AllowedHostsValidator { get; } 
    public RestfulBookerAccessTokenProvider(string baseUrl, AuthParams authParams)
    {
        _baseUrl = baseUrl;
        _authParams = authParams;
        _httpClient = new HttpClient();
        
        var baseUri = new Uri(baseUrl);
        _httpClient.BaseAddress = baseUri;
        
        AllowedHostsValidator = new AllowedHostsValidator(new[] { baseUri.Host });
    }
    
    public async Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (!AllowedHostsValidator.IsUrlHostValid(uri))
        {
            throw new ArgumentException($"The host {uri.Host} is not allowed by the current configuration");
        }

        var path = uri.AbsolutePath.ToLowerInvariant();
        // Prevent infinite recursion
        if (path.Contains("/auth"))
        {
            return string.Empty;
        }

        var method = additionalAuthenticationContext?["method"];
        if (method is Method.PUT or Method.PATCH or Method.DELETE)
        {
            var token = await GetTokenAsync(cancellationToken);
            return token;
        }
        return string.Empty;
    }

    private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        if (_token is not null)
        {
            return _token;
        }
        try
        {
            var authUrl = $"{_baseUrl}/auth";

            var response = await _httpClient.PostAsJsonAsync(authUrl, _authParams, cancellationToken);
            response.EnsureSuccessStatusCode();

            var authResponse = await JsonSerializer.DeserializeAsync<AuthResponse>(
                await response.Content.ReadAsStreamAsync(cancellationToken),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            if (authResponse?.Token != null)
            {
                _token = authResponse.Token;
                return _token;
            }

            throw new InvalidOperationException("Failed to obtain authentication token");
        }
        catch (Exception ex)
        {
            // In a production environment, you might want to log this error
            Console.WriteLine($"Authentication error: {ex.Message}");
            throw;
        }
    }
}