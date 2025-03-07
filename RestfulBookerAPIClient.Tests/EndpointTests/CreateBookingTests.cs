using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;

namespace RestfulBookerAPIClient.Tests.EndpointTests;

[Collection("BookingTests")]
public class CreateBookingTests
{
    private readonly RestfulBookerClient _client;
    private readonly BookingsFixture _bookingsFixture;
    
    public CreateBookingTests(ApiClientFixture fixture, BookingsFixture bookingsFixture)
    {
        _client = fixture.Client;
        _bookingsFixture = bookingsFixture;
    }
    
    [Fact]
    public async Task CreateBookingReturnsOk()
    {
        var booking = _bookingsFixture.GoodBooking; 

        var response = await _client.Booking.PostAsync(booking);

        response.Should().NotBeNull();
        response.Bookingid.Should().NotBeNull("because the client should return a booking id");
    }

    [Fact]
    public async Task CreateBookingWithInvalidDatesReturnsBadRequest()
    {
        var booking = _bookingsFixture.BadBooking;

        await _client.Invoking(async x => await x.Booking.PostAsync(booking))
            .Should().ThrowAsync<ApiException>("because given booking data is invalid")
            .Where(e => e.ResponseStatusCode == 500, "because the expected error code is INTERNAL SERVER ERROR");
    }
}