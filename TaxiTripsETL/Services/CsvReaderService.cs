using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class CsvReaderService
{
    public IEnumerable<TaxiTrip> ReadTrips(string filePath, Action? onInvalidRow = null)
    {
        using var streamReader = new StreamReader(filePath);
        using var csv = new CsvReader(streamReader, GetConfig());

        csv.Context.RegisterClassMap<TaxiTripMap>();

        while (csv.Read())
        {
            TaxiTrip? record = null;

            try
            {
                record = csv.GetRecord<TaxiTrip>();
            }
            catch
            {
                onInvalidRow?.Invoke();
            }

            if (record != null)
            {
                yield return record;
            }
        }
    }

    private static CsvConfiguration GetConfig() =>
        new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null
        };
}
