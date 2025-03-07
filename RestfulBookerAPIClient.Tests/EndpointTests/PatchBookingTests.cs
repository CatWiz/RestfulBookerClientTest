using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;
using RestfulBookerAPIClient.Tests.Utils;

namespace RestfulBookerAPIClient.Tests.EndpointTests;

[Collection("BookingTests")]
public class PatchBookingTests
{
    private readonly RestfulBookerClient _client;
    private readonly BookingsFixture _bookingsFixture;
    
    public PatchBookingTests(ApiClientFixture fixture, BookingsFixture bookingsFixture)
    {
        _client = fixture.Client;
        _bookingsFixture = bookingsFixture;
    }
    
    [Fact]
    public async Task PatchBookingRunsOk()
    {
        var bookingId = await BookingUtils.GetRandomBookingId(_client);
        bookingId.Should().NotBeNull("because there should be at least one booking available");

        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        var response = await _client.Booking[(int)bookingId].PatchAsync(bookingPatch);

        response.Should().NotBeNull();
        foreach (var property in bookingPatch.GetType().GetProperties())
        {
            var patchValue = property.GetValue(bookingPatch);
            if (patchValue is null) continue;
            
            var subjectValue = property.GetValue(response);
            subjectValue.Should().BeEquivalentTo(patchValue, "because the updated booking should have the same values as the patch");
        }
    }

    [Fact]
    public async Task PatchNonExistentBookingReturnsNotAllowed()
    {
        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        
        await _client.Invoking(x => x.Booking[-1].PatchAsync(bookingPatch))
            .Should().ThrowAsync<ApiException>()
            .Where(e => e.ResponseStatusCode == 405, "because the expected error code is METHOD NOT ALLOWED");
    }
}