using System.Configuration;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Extensions.Configuration;

namespace RestfulBookerAPIClient.Tests;

public static class TestConfig
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build(); 
    
    private static string GetConfigValue(string key) => Config[key]
                                                        ?? throw new ConfigurationErrorsException($"{key} is not set in the configuration file");

    public static readonly string BaseUrl = GetConfigValue("BaseUrl");

    public static readonly AuthParams BadAuthParams = new()
    {
        Username = GetConfigValue("BadAuthUsername"),
        Password = GetConfigValue("BadAuthPassword")
    };
    
    public static readonly AuthParams GoodAuthParams = new()
    {
        Username = GetConfigValue("GoodAuthUsername"),
        Password = GetConfigValue("GoodAuthPassword")
    };
}