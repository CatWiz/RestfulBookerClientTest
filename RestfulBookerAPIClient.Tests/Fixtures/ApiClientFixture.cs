using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Http.HttpClientLibrary;
using RestfulBookerAPIClient.Auth;

namespace RestfulBookerAPIClient.Tests.Fixtures;

public class ApiClientFixture
{
    public RestfulBookerClient Client { get; }
    
    public ApiClientFixture()
    {
        var tokenProvider = new RestfulBookerAccessTokenProvider(TestConfig.BaseUrl, TestConfig.GoodAuthParams);
        var authProvider = new RestfulBookerAuthenticationProvider(tokenProvider);
        var adapter = new HttpClientRequestAdapter(authProvider);
        adapter.BaseUrl = TestConfig.BaseUrl;
        Client = new RestfulBookerClient(adapter);
    }
}

[CollectionDefinition("ApiClient")]
public class ApiClientCollection : ICollectionFixture<ApiClientFixture> { }
