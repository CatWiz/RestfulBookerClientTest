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

    var userId = 1;
    var someBooking = await client.Booking[userId].GetAsync();
    if (someBooking is not null)
    {
        Console.WriteLine($"Found a booking for user {userId}:");
        Console.WriteLine($"{someBooking.Firstname} {someBooking.Lastname}");
        Console.WriteLine($"{someBooking.Bookingdates?.Checkin} - {someBooking.Bookingdates?.Checkout}");
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}