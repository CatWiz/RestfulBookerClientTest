using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace RestfulBookerAPIClient.Tests.Fixtures;

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
