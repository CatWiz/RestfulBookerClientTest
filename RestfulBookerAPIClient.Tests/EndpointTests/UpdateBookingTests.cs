using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;
using RestfulBookerAPIClient.Tests.Utils;

namespace RestfulBookerAPIClient.Tests.EndpointTests;

[Collection("BookingTests")]
public class UpdateBookingTests
{
    private readonly RestfulBookerClient _client;
    private readonly BookingsFixture _bookingsFixture;

    public UpdateBookingTests(ApiClientFixture fixture, BookingsFixture bookingsBookingsFixture)
    {
        _client = fixture.Client;
        _bookingsFixture = bookingsBookingsFixture;
    }
    
    [Fact]
    public async Task UpdateBookingRunsOk()
    {
        var newBooking = _bookingsFixture.GoodUpdateBooking;
        var bookingId = await BookingUtils.GetRandomBookingId(_client);
        bookingId.Should().NotBeNull("because there should be at least one booking available");
        
        var response = await _client.Booking[(int)bookingId].PutAsync(newBooking);
        
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(newBooking, "because the updated booking should be same as passed data");
    }
    
    [Fact]
    public async Task UpdateNonExistentBookingReturnsNotAllowed()
    {
        var newBooking = _bookingsFixture.GoodUpdateBooking;
        await _client.Invoking(async x => await x.Booking[-1].PutAsync(newBooking))
            .Should().ThrowAsync<ApiException>("because the booking id does not exist")
            .Where(e => e.ResponseStatusCode == 405, "because the expected error code is METHOD NOT ALLOWED");
    }

    [Fact]
    public async Task UpdateBookingWithInvalidDataReturnsBadRequest()
    {
        var newBooking = _bookingsFixture.BadUpdateBooking;
        
        await _client.Invoking(async x => await x.Booking[1].PutAsync(newBooking))
            .Should().ThrowAsync<ApiException>("because given booking data is invalid")
            .Where(e => e.ResponseStatusCode == 400, "because the error code should be BAD REQUEST");
    }
}