using KiotaPosts.RestfulBookerClient.Models;
using Microsoft.Kiota.Abstractions;

namespace RestfulBookerAPIClient.Tests.Fixtures;

public class BookingsFixture
{
    // Booking with all the correct fields
    public readonly Booking GoodBooking = new()
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
    
    // Booking with some fields missing
    public readonly Booking BadBooking = new()
    {
        // Firstname = "John",
        // Lastname = "Doe",
        Totalprice = 100,
        Depositpaid = true,
        Additionalneeds = "Breakfast",
        Bookingdates = new Booking_bookingdates()
        {
            Checkin = new Date(2024, 12, 27),
            Checkout = new Date(2025, 1, 3)
        },
    };
    
    // Booking with all the correct fields that's distinct from GoodBooking
    // This is used to test the PUT method
    public readonly Booking GoodUpdateBooking = new()
    {
        Firstname = "Vasyl",
        Lastname = "Petrenko",
        Totalprice = 47,
        Depositpaid = true,
        Additionalneeds = "Many",
        Bookingdates = new Booking_bookingdates()
        {
            Checkin = new Date(2024, 12, 27),
            Checkout = new Date(2025, 1, 3)
        },
    };

}

[CollectionDefinition("BookingTests")]
public class BookingsCollection : ICollectionFixture<BookingsFixture>, ICollectionFixture<ApiClientFixture> { }