using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;

namespace RestfulBookerAPIClient.Tests.BookingTests;

[Collection("BookingTests")]
public class BookingTests
{
    private readonly RestfulBookerClient _client;
    private readonly BookingsFixture _bookingsFixture;

    public BookingTests(ApiClientFixture fixture, BookingsFixture bookingsFixture)
    {
        _client = fixture.Client;
        _bookingsFixture = bookingsFixture;
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
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var booking = await _client.Booking[-1].GetAsync();
        });

        Assert.Equal(404, exception.ResponseStatusCode);
    }

    [Fact]
    public async Task CreateBookingReturnsOk()
    {
        var booking = _bookingsFixture.GoodBooking; 

        var response = await _client.Booking.PostAsync(booking);

        Assert.NotNull(response);
        Assert.NotNull(response.Bookingid);
    }

    [Fact]
    public async Task CreateBookingWithInvalidDatesReturnsBadRequest()
    {
        var booking = _bookingsFixture.BadBooking;

        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var response = await _client.Booking.PostAsync(booking);
        });

        Assert.Equal(500, exception.ResponseStatusCode);
    }

    [Fact]
    public async Task UpdateBookingRunsOk()
    {
        var newBooking = _bookingsFixture.GoodUpdateBooking;
            
        var bookingsList = await _client.Booking.GetAsync();
        var bookingId = bookingsList?.First().Bookingid;
        Assert.NotNull(bookingId);
        
        var response = await _client.Booking[(int)bookingId].PutAsync(newBooking);
        
        Assert.NotNull(response);
        Assert.Equal(newBooking.Firstname, response.Firstname);
        Assert.Equal(newBooking.Lastname, response.Lastname);
        Assert.Equal(newBooking.Totalprice, response.Totalprice);
        Assert.Equal(newBooking.Depositpaid, response.Depositpaid);
        Assert.Equal(newBooking.Additionalneeds, response.Additionalneeds);
        Assert.NotNull(response.Bookingdates);
        Assert.Equal(newBooking.Bookingdates?.Checkin, response.Bookingdates.Checkin);
        Assert.Equal(newBooking.Bookingdates?.Checkout, response.Bookingdates.Checkout);
    }
    
    [Fact]
    public async Task UpdateNonExistentBookingReturnsNotAllowed()
    {
        var newBooking = _bookingsFixture.GoodUpdateBooking;
        
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var response = await _client.Booking[-1].PutAsync(newBooking);
        });

        Assert.Equal(405, exception.ResponseStatusCode);
    }

    [Fact]
    public async Task UpdateBookingWithInvalidDataReturnsBadRequest()
    {
        var newBooking = _bookingsFixture.BadUpdateBooking;
        
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var response = await _client.Booking[1].PutAsync(newBooking);
        });
        
        Assert.Equal(400, exception.ResponseStatusCode);
    }
    
    [Fact]
    public async Task DeleteBookingRunsOk()
    {
        var bookingsList = await _client.Booking.GetAsync();
        var bookingId = bookingsList?.First().Bookingid;
        Assert.NotNull(bookingId);
        
        var response = await _client.Booking[(int)bookingId].DeleteAsync();
        
        Assert.NotNull(response);
        Assert.Equal("Created", response);
    }
    
    [Fact]
    public async Task DeleteNonExistentBookingReturnsNotAllowed()
    {
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var response = await _client.Booking[-1].DeleteAsync();
        });

        Assert.Equal(405, exception.ResponseStatusCode);
    }

    [Fact]
    public async Task PartialUpdateBookingRunsOk()
    {
        var bookingsList = await _client.Booking.GetAsync();
        var bookingId = bookingsList?.First().Bookingid;
        Assert.NotNull(bookingId);

        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        var response = await _client.Booking[(int)bookingId].PatchAsync(bookingPatch);
        
        Assert.NotNull(response);
        Assert.True(bookingPatch.Firstname is null || bookingPatch.Firstname == response.Firstname);
        Assert.True(bookingPatch.Lastname is null || bookingPatch.Lastname == response.Lastname);
        Assert.True(bookingPatch.Totalprice is null || bookingPatch.Totalprice == response.Totalprice);
        Assert.True(bookingPatch.Depositpaid is null || bookingPatch.Depositpaid == response.Depositpaid);
        Assert.True(bookingPatch.Additionalneeds is null || bookingPatch.Additionalneeds == response.Additionalneeds);
        Assert.True(bookingPatch.Bookingdates?.Checkin is null ||  bookingPatch.Bookingdates.Checkin.Equals(response.Bookingdates?.Checkin));
        Assert.True(bookingPatch.Bookingdates?.Checkout is null || bookingPatch.Bookingdates.Checkout.Equals(response.Bookingdates?.Checkout));
    }
    
    [Fact]
    public async Task PartialUpdateNonExistentBookingReturnsNotAllowed()
    {
        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var response = await _client.Booking[-1].PatchAsync(bookingPatch);
        });

        Assert.Equal(405, exception.ResponseStatusCode);
    }
}