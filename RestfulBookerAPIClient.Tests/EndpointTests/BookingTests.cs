using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;

namespace RestfulBookerAPIClient.Tests.PingTests;

[Collection("ApiClient")]
public class BookingTests
{
    private readonly RestfulBookerClient _client;
    
    public BookingTests(ApiClientFixture fixture)
    {
        _client = fixture.Client;
    }
    
    [Fact]
    public async Task GetAllBookingsReturnsOk()
    {
        var bookingsList = await _client.Booking.GetAsync();
        
        Assert.NotNull(bookingsList);
        Assert.NotEmpty(bookingsList);
        Assert.NotNull(bookingsList.First().Bookingid);
    }
    
    [Fact]
    public async Task GetBookingByIdReturnsOk()
    {
        var bookingsList = await _client.Booking.GetAsync();
        var bookingId = bookingsList?.First().Bookingid;
        Assert.NotNull(bookingId);
        
        var booking = await _client.Booking[(int)bookingId].GetAsync();
        
        Assert.NotNull(booking);
        Assert.NotNull(booking.Firstname);
        Assert.NotNull(booking.Lastname);
        Assert.NotNull(booking.Bookingdates);
        Assert.NotNull(booking.Bookingdates.Checkin);
        Assert.NotNull(booking.Bookingdates.Checkout);
    }
    
    [Fact]
    public async Task GetBookingByNonExistentIdReturnsNull()
    {
        var exception = await Assert.ThrowsAsync<ApiException>( async () =>
        {
            var booking = await _client.Booking[-1].GetAsync();       
        });
        
        Assert.Equal(404, exception.ResponseStatusCode);
    }
}