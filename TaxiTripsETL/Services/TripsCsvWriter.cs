using System.Globalization;
using CsvHelper;
using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class TripsCsvWriter : IDisposable
{
    private readonly StreamWriter _streamWriter;
    private readonly CsvWriter _csvWriter;

    public TripsCsvWriter(string filePath)
    {
        _streamWriter = new StreamWriter(filePath);
        _csvWriter = new CsvWriter(_streamWriter, CultureInfo.InvariantCulture);
    }
    
    public void Write(TaxiTrip record)
    {
        _csvWriter.WriteRecord(record);
        _csvWriter.NextRecord();
    }

    public void Dispose()
    {
        _csvWriter.Dispose();
        _streamWriter.Dispose();
    }
}