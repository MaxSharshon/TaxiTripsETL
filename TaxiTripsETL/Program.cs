using TaxiTripsETL.Services;
using TaxiTripsETL;

var csvPath = args.Length > 0
    ? args[0]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data", "sample-cab-data.csv"));

var duplicatesPath = args.Length > 1 ? args[1] : "duplicates.csv";
var connectionString = Environment.GetEnvironmentVariable("TAXI_SQL_CONNECTION");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Environment variable TAXI_SQL_CONNECTION is required.");
    Console.WriteLine("Example: Server=localhost,1433;Database=TaxiTripsDb;User Id=sa;Password=your_password;TrustServerCertificate=True;Encrypt=False");
    return;
}

var app = new App(
    new CsvReaderService(),
    new DuplicateDetector(),
    new TripTransformer(),
    new SqlBulkInsertService(connectionString));

app.Run(csvPath, duplicatesPath);
