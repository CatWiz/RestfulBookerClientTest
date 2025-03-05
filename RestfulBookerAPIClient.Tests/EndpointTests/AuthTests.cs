using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;

namespace RestfulBookerAPIClient.Tests.PingTests;

[Collection("ApiClient")]
public class AuthTests
{
    private readonly RestfulBookerClient _client;
    
    public AuthTests(ApiClientFixture fixture)
    {
        _client = fixture.Client;
    }
    
    [Fact]
    private async Task AuthWithBadCredentialsFails() {
        var response = await _client.Auth.PostAsync(TestConfig.BadAuthParams);
     
        Assert.NotNull(response);
        Assert.Null(response.Token);
        Assert.NotNull(response.Reason);
    }
    
    [Fact]
    private async Task AuthWithGoodCredentialsSucceeds() {
        var response = await _client.Auth.PostAsync(TestConfig.GoodAuthParams);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Token);
        Assert.Null(response.Reason);
    }
    
    [Fact]
    private async Task AuthWithEmptyCredentialsFails() {
        var response = await _client.Auth.PostAsync(new AuthParams());
        
        Assert.NotNull(response);
        Assert.Null(response.Token);
        Assert.NotNull(response.Reason);
    }
}