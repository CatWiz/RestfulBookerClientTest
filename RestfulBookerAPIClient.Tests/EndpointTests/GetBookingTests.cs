using FluentAssertions;
using KiotaPosts.RestfulBookerClient;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;
using RestfulBookerAPIClient.Tests.Utils;

namespace RestfulBookerAPIClient.Tests.EndpointTests;

[Collection("ApiClient")]
public class GetBookingTests
{
    private readonly RestfulBookerClient _client;

    public GetBookingTests(ApiClientFixture fixture)
    {
        _client = fixture.Client;
    }
    
    [Fact]
    public async Task GetAllBookingsReturnsOk()
    {
        var bookingsList = await _client.Booking.GetAsync();

        bookingsList.Should().NotBeNullOrEmpty("because there should be bookings available");
        bookingsList.Should().AllSatisfy(bookingData =>
        {
            bookingData.Bookingid.Should().NotBeNull("because all bookings should have an id");
        });
    }
    
    [Fact]
    public async Task GetBookingByIdReturnsOk()
    {
        var bookingId = await BookingUtils.GetRandomBookingId(_client);
        bookingId.Should().NotBeNull("because there should be a booking id available");
        var booking = await _client.Booking[(int)bookingId].GetAsync();

        booking.Should().NotBeNull("because the client should return a booking");
        booking.Firstname.Should().NotBeNullOrEmpty("because the booking should have a first name");
        booking.Lastname.Should().NotBeNullOrEmpty("because the booking should have a last name");
        booking.Totalprice.Should().NotBeNull("because the booking should have a total price");
        booking.Depositpaid.Should().NotBeNull("because the booking should have a deposit paid status");
        booking.Bookingdates.Should().NotBeNull("because the booking should have booking dates");
        booking.Bookingdates.Checkin.Should().NotBeNull("because the booking should have a check-in date");
        booking.Bookingdates.Checkout.Should().NotBeNull("because the booking should have a check-out date");
    }

    [Fact]
    public async Task GetBookingByNonExistentIdReturnsNull()
    {
        await _client.Invoking(async x => await x.Booking[-1].GetAsync())
            .Should().ThrowAsync<ApiException>("because the booking id does not exist")
            .Where(e => e.ResponseStatusCode == 404, "because the error code should be NOT FOUND");
    }
}