using Xunit;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using KiotaPosts.RestfulBookerClient;
using System.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace RestfulBookerAPIClient.Tests;

public class ApiClientFixture
{
    public RestfulBookerClient Client { get; }
    
    public ApiClientFixture()
    {
        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider);
        adapter.BaseUrl = TestConfig.BaseUrl;
        Client = new RestfulBookerClient(adapter);
    }
}

[CollectionDefinition("ApiClient")]
public class ApiClientCollection : ICollectionFixture<ApiClientFixture> { }

public class UnitTest1
{
    [Fact]
    public void SmokeTest()
    {
        int a = 5;
        Assert.Equal(10, a * 2);
    }
}