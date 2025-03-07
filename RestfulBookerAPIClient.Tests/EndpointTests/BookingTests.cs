using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions;
using RestfulBookerAPIClient.Tests.Fixtures;
using FluentAssertions;
using System.Reflection;

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

        bookingsList.Should().NotBeNullOrEmpty("because there should be bookings available");
        bookingsList.Should().AllSatisfy(bookingData =>
        {
            bookingData.Bookingid.Should().NotBeNull("because all bookings should have an id");
        });
    }
    
    private int GetRandomBookingId()
    {
        var bookingsList = _client.Booking.GetAsync().Result;
        var randomBooking = bookingsList?.First().Bookingid;
        randomBooking.Should().NotBeNull("because there should be at least one booking available");
        
        return (int)randomBooking;
    }
    
    [Fact]
    public async Task GetBookingByIdReturnsOk()
    {
        var bookingId = GetRandomBookingId();
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

    [Fact]
    public async Task UpdateBookingRunsOk()
    {
        var newBooking = _bookingsFixture.GoodUpdateBooking;
        var bookingId = GetRandomBookingId();
        
        var response = await _client.Booking[bookingId].PutAsync(newBooking);
        
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
    
    [Fact]
    public async Task DeleteBookingRunsOk()
    {
        var bookingId = GetRandomBookingId();
        
        var response = await _client.Booking[bookingId].DeleteAsync();
        
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

    [Fact]
    public async Task PartialUpdateBookingRunsOk()
    {
        var bookingId = GetRandomBookingId();

        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        var response = await _client.Booking[bookingId].PatchAsync(bookingPatch);

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
    public async Task PartialUpdateNonExistentBookingReturnsNotAllowed()
    {
        var bookingPatch = _bookingsFixture.GoodPartialUpdateBooking;
        
        await _client.Invoking(x => x.Booking[-1].PatchAsync(bookingPatch))
            .Should().ThrowAsync<ApiException>()
            .Where(e => e.ResponseStatusCode == 405, "because the expected error code is METHOD NOT ALLOWED");
    }
}