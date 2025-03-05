using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;

namespace RestfulBookerAPIClient.Tests.BookingTests;

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
    
    [Fact]
    public async Task CreateBookingReturnsOk()
    {
        var booking = new Booking()
        {
            Firstname = "John",
            Lastname = "Doe",
            Totalprice = 100,
            Depositpaid = true,
            Additionalneeds = "Breakfast",
            Bookingdates = new Booking_bookingdates()
            {
                Checkin = new Date(2024, 12, 27),
                Checkout = new Date(2025, 1, 3)
            },
        };
        
        var response = await _client.Booking.PostAsync(booking);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Bookingid);
    }
    
    [Fact]
    public async Task CreateBookingWithInvalidDatesReturnsBadRequest()
    {
        var booking = new Booking()
        {
            // Skip some required fields
            // Firstname = "John",
            // Lastname = "Doe",
            Totalprice = 100,
            Depositpaid = true,
            Additionalneeds = "Breakfast",
            Bookingdates = new Booking_bookingdates()
            {
                Checkin = new Date(2024, 12, 27),
                Checkout = new Date(2024, 12, 26)
            },
        };
        
        var exception = await Assert.ThrowsAsync<ApiException>( async () =>
        {
            var response = await _client.Booking.PostAsync(booking);
        });
        
        Assert.Equal(400, exception.ResponseStatusCode);
    }
}