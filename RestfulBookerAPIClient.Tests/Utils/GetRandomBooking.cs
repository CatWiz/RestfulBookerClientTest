using KiotaPosts.RestfulBookerClient;
using KiotaPosts.RestfulBookerClient.Models;

namespace RestfulBookerAPIClient.Tests.Utils;

public static class BookingUtils
{
    public static async Task<int?> GetRandomBookingId(RestfulBookerClient client)
    {
        var response = await client.Booking.GetAsync();
        return response?[response.Count / 2].Bookingid;
    }
}