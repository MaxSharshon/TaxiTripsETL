using System.Data;
using Microsoft.Data.SqlClient;
using TaxiTripsETL.Models;

namespace TaxiTripsETL.Services;

public class SqlBulkInsertService(string connectionString)
{
    public int InsertTrips(IEnumerable<TaxiTrip> trips, int batchSize = 5000)
    {
        var inserted = 0;
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var bulkCopy = BuildBulkCopy(connection);
        var table = BuildTripsDataTable();

        foreach (var trip in trips)
        {
            table.Rows.Add(
                trip.PickupDatetime,
                trip.DropoffDatetime,
                ToDbValue(trip.PassengerCount),
                ToDbValue(trip.TripDistance),
                ToDbValue(trip.StoreAndForwardFlag),
                ToDbValue(trip.PickupLocationId),
                ToDbValue(trip.DropoffLocationId),
                ToDbValue(trip.FareAmount),
                ToDbValue(trip.TipAmount));

            inserted++;

            if (table.Rows.Count >= batchSize)
            {
                bulkCopy.WriteToServer(table);
                table.Clear();
            }
        }

        if (table.Rows.Count > 0)
        {
            bulkCopy.WriteToServer(table);
        }

        return inserted;
    }

    public int GetTaxiTripsCount()
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT COUNT_BIG(1) FROM dbo.TaxiTrips;", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static SqlBulkCopy BuildBulkCopy(SqlConnection connection)
    {
        var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
        {
            DestinationTableName = "dbo.TaxiTrips",
            BatchSize = 5000,
            BulkCopyTimeout = 0,
            EnableStreaming = true
        };

        bulkCopy.ColumnMappings.Add("PickupDatetime", "PickupDatetime");
        bulkCopy.ColumnMappings.Add("DropoffDatetime", "DropoffDatetime");
        bulkCopy.ColumnMappings.Add("PassengerCount", "PassengerCount");
        bulkCopy.ColumnMappings.Add("TripDistance", "TripDistance");
        bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "StoreAndFwdFlag");
        bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
        bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
        bulkCopy.ColumnMappings.Add("FareAmount", "FareAmount");
        bulkCopy.ColumnMappings.Add("TipAmount", "TipAmount");

        return bulkCopy;
    }

    private static DataTable BuildTripsDataTable()
    {
        var table = new DataTable();

        table.Columns.Add("PickupDatetime", typeof(DateTime));
        table.Columns.Add("DropoffDatetime", typeof(DateTime));
        table.Columns.Add("PassengerCount", typeof(int));
        table.Columns.Add("TripDistance", typeof(double));
        table.Columns.Add("StoreAndFwdFlag", typeof(string));
        table.Columns.Add("PULocationID", typeof(int));
        table.Columns.Add("DOLocationID", typeof(int));
        table.Columns.Add("FareAmount", typeof(decimal));
        table.Columns.Add("TipAmount", typeof(decimal));

        return table;
    }

    private static object ToDbValue<T>(T? value) where T : struct
    {
        return value.HasValue ? value.Value : DBNull.Value;
    }

    private static object ToDbValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;
    }
}
