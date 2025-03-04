using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace RestfulBookerAPIClient.Tests;

public static class TestConfig
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build(); 
    
    public static readonly string BaseUrl = Config["BaseUrl"]
                                            ?? throw new ConfigurationErrorsException("BaseUrl is not set in the configuration file");
}