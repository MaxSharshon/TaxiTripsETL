using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class DuplicateDetector
{
    public IEnumerable<TaxiTrip> Detect(
        IEnumerable<TaxiTrip> records,
        Action<TaxiTrip> onDuplicateFound)

    {
        var seen = new HashSet<(DateTime, DateTime, int?)>();

        foreach (var record in records)
        {
            var key = (
                record.PickupDatetime,
                record.DropoffDatetime,
                record.PassengerCount);

            if (!seen.Add(key))
            {
                onDuplicateFound(record);
            }
            else
            {
                yield return record;
            }
        }
    }
}