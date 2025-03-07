using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;
using RestfulBookerAPIClient.Tests.Utils;

namespace RestfulBookerAPIClient.Tests.EndpointTests;

[Collection("BookingTests")]
public class DeleteBookingTests
{
    private readonly RestfulBookerClient _client;
    private readonly BookingsFixture _bookingsFixture;
    
    public DeleteBookingTests(ApiClientFixture fixture, BookingsFixture bookingsFixture)
    {
        _client = fixture.Client;
        _bookingsFixture = bookingsFixture;
    }
    
    [Fact]
    public async Task DeleteBookingRunsOk()
    {
        var bookingId = await BookingUtils.GetRandomBookingId(_client);
        bookingId.Should().NotBeNull("because there should be at least one booking available");
        
        var response = await _client.Booking[(int)bookingId].DeleteAsync();
        
        response.Should().NotBeNull();
        response.Should().Be("Created", "because that is the expected response from the server");
    }
    
    [Fact]
    public async Task DeleteNonExistentBookingReturnsNotAllowed()
    {
        await _client.Invoking(x => x.Booking[-1].DeleteAsync())
            .Should().ThrowAsync<ApiException>("because the booking does not exist")
            .Where(e => e.ResponseStatusCode == 405, "because the expected error code is METHOD NOT ALLOWED");
    }
}