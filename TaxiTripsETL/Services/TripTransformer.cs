using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class TripTransformer
{
    private static readonly TimeZoneInfo EstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public TaxiTrip Normalize(TaxiTrip trip)
    {
        trip.StoreAndForwardFlag = NormalizeStoreAndForwardFlag(trip.StoreAndForwardFlag);
        trip.PickupDatetime = ConvertEstToUtc(trip.PickupDatetime);
        trip.DropoffDatetime = ConvertEstToUtc(trip.DropoffDatetime);

        return trip;
    }

    private static string NormalizeStoreAndForwardFlag(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            return "No";
        }

        if (normalized.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            return "Yes";
        }

        return normalized;
    }

    private static DateTime ConvertEstToUtc(DateTime value)
    {
        var unspecified = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, EstTimeZone);
    }
}
