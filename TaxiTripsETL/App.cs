using TaxiTripsETL.Services;

namespace TaxiTripsETL;

public class App(
    CsvReaderService csvReader,
    DuplicateDetector duplicateDetector,
    TripTransformer tripTransformer,
    SqlBulkInsertService bulkInserter)
{
    public void Run(string csvPath, string duplicatesPath)
    {
        if (!File.Exists(csvPath))
        {
            Console.WriteLine($"Input CSV was not found: {csvPath}");
            return;
        }

        var invalidRows = 0;
        var duplicateRows = 0;

        using var duplicateWriter = new TripsCsvWriter(duplicatesPath);

        var uniqueTrips = duplicateDetector.Detect(
            csvReader.ReadTrips(csvPath, () => invalidRows++).Select(tripTransformer.Normalize),
            duplicate =>
            {
                duplicateRows++;
                duplicateWriter.Write(duplicate);
            });

        var insertedRows = bulkInserter.InsertTrips(uniqueTrips);
        var tableRows = bulkInserter.GetTaxiTripsCount();

        Console.WriteLine($"Inserted rows: {insertedRows}");
        Console.WriteLine($"Duplicate rows written to '{duplicatesPath}': {duplicateRows}");
        Console.WriteLine($"Invalid rows skipped: {invalidRows}");
        Console.WriteLine($"Rows currently in dbo.TaxiTrips: {tableRows}");
    }
}
