using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;
using RestfulBookerAPIClient.Tests.Fixtures;
using FluentAssertions;

namespace RestfulBookerAPIClient.Tests.AuthTests;

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

        response.Should().NotBeNull();
        response.Token.Should().BeNull("because the credentials are invalid");
        response.Reason.Should().NotBeNullOrEmpty("because the credentials are invalid");
    }
    
    [Fact]
    private async Task AuthWithGoodCredentialsSucceeds() {
        var response = await _client.Auth.PostAsync(TestConfig.GoodAuthParams);
        
        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrEmpty("because the credentials are valid");
        response.Reason.Should().BeNullOrEmpty("because the credentials are valid");
    }
    
    [Fact]
    private async Task AuthWithEmptyCredentialsFails() {
        var response = await _client.Auth.PostAsync(new AuthParams());
        
        response.Should().NotBeNull();
        response.Token.Should().BeNull("because the credentials are empty");
        response.Reason.Should().NotBeNullOrEmpty("because the credentials are empty");
    }
}