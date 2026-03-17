using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class CsvReaderService
{
    public IEnumerable<TaxiTrip> ReadTrips(string filePath)
    {
        using var streamReader = new StreamReader(filePath);
        using var csvReader = new CsvReader(streamReader, GetConfig());

        csvReader.Context.RegisterClassMap<TaxiTripMap>();
        
        foreach (var record in csvReader.GetRecords<TaxiTrip>())
            yield return record;
    }

    private CsvConfiguration GetConfig() =>
        new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null
        };
}