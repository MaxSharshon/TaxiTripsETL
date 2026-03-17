# TaxiTrips ETL

CLI ETL tool that imports NYC taxi trips from CSV into SQL Server with deduplication and bulk insertion.

## What It Does

- Reads and validates CSV rows from `sample-cab-data.csv`.
- Keeps only the required columns from the assessment.
- Cleans text fields (trim spaces).
- Converts `store_and_fwd_flag`: `N -> No`, `Y -> Yes`.
- Converts pickup/dropoff datetimes from EST to UTC before insert.
- Detects duplicates by `(pickup_datetime, dropoff_datetime, passenger_count)`.
- Writes duplicates to `duplicates.csv`.
- Bulk inserts unique rows into `dbo.TaxiTrips` using `SqlBulkCopy`.

## Tech Stack

- C# / .NET 8
- SQL Server 2022 (Docker)
- CsvHelper
- Microsoft.Data.SqlClient

## Project Structure

- `TaxiTripsETL/` - Console ETL application
- `sql/create_database.sql` - Create database
- `sql/create_trips_table.sql` - Create target table
- `sql/create_indexes.sql` - Create optimized indexes
- `sql/queries.sql` - Query examples requested by assessment
- `docker-compose.yaml` - SQL Server + optional ETL container

## Setup

1. Set `.env`:

```env
MSSQL_SA_PASSWORD=your_password
```

2. Start SQL Server:

```powershell
docker compose up -d sqlserver
```

3. Run SQL scripts in order:

1. `sql/create_database.sql`
2. `sql/create_trips_table.sql`
3. `sql/create_indexes.sql`

4. Set app connection string in your shell:

```powershell
$env:TAXI_SQL_CONNECTION = "Server=localhost,1433;Database=TaxiTripsDb;User Id=sa;Password=your_password;TrustServerCertificate=True;Encrypt=False"
```

5. Run ETL:

```powershell
dotnet run --project .\TaxiTripsETL\TaxiTripsETL.csproj -- .\data\sample-cab-data.csv .\duplicates.csv
```

Optional: run ETL in Docker (connection string is set in `docker-compose.yaml`):

```powershell
docker compose run --rm etl
```

The app prints:
- inserted row count
- duplicate row count
- invalid row count
- current table row count

## Assumptions

- Input datetime values are EST and are converted to UTC during transformation.
- Invalid rows are skipped (rather than failing the entire job).
- `store_and_fwd_flag` outside `N/Y` is trimmed and inserted as-is (or `NULL` when empty).

## Performance and 10GB Input Notes

For a 10GB CSV, I would change the pipeline to:

- Use `SqlBulkCopy` with a custom `IDataReader` to avoid `DataTable` allocations.
- Replace in-memory duplicate `HashSet` with a staging table and SQL-side deduplication using a unique index on the dedup key.
- Run with larger batch sizes and optional partitioned loading.
- Add resumable checkpoints (file offset / batch id) for fault tolerance.
- Add structured logging and metrics for throughput and bad-row rates.

## Required Query Support

The schema/indexes are optimized for:

- Highest average `tip_amount` by `PULocationID`
- Top 100 by `trip_distance`
- Top 100 by travel time (`TripDurationSeconds` computed column)
- Search by `PULocationID`

See `sql/queries.sql` for runnable SQL examples.
