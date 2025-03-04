using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

var authProvider = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(authProvider);
adapter.BaseUrl = "https://restful-booker.herokuapp.com";
var client = new RestfulBookerClient(adapter);

try
{
    string? pingResponse = await client.Ping.GetAsync();
    Console.WriteLine($"Ping response: {pingResponse}");

    var allBookings = await client.Booking.GetAsync();
    var randomBooking = allBookings?.First().Bookingid ?? throw new Exception();

    var someBooking = await client.Booking[randomBooking].GetAsync();
    if (someBooking is not null)
    {
        Console.WriteLine($"Found a booking for user {randomBooking}:");
        Console.WriteLine($"{someBooking.Firstname} {someBooking.Lastname}");
        Console.WriteLine($"{someBooking.Bookingdates?.Checkin} - {someBooking.Bookingdates?.Checkout}");
    }

    var authParams = new AuthParams()
    {
        Username = "admin",
        Password = "password123"
    };
    var response = await client.Auth.PostAsync(authParams);
    if (response is not null)
    {
        if (response.Token is not null)
        {
            Console.WriteLine($"Authenticated successfully: {response.Token}");
        }
        else
        {
            Console.WriteLine($"Authentication failed: {response.Reason}");
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}