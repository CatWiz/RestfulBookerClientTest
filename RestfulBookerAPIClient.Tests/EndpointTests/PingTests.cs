using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using RestfulBookerAPIClient.Tests.Fixtures;

namespace RestfulBookerAPIClient.Tests.PingTests;

[Collection("ApiClient")]
public class RestfulBookerClientPingTests
{
    private readonly RestfulBookerClient _client;
    
    public RestfulBookerClientPingTests(ApiClientFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task PingRunsOk()
    {
        var response = await _client.Ping.GetAsync();

        response.Should().NotBeNull();
    }
}