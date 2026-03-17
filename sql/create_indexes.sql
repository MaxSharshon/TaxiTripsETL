USE TaxiTripsDb;
GO

SET QUOTED_IDENTIFIER ON;
GO

SET ANSI_NULLS ON;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TaxiTrips_PULocationID' AND object_id = OBJECT_ID('dbo.TaxiTrips'))
    DROP INDEX IX_TaxiTrips_PULocationID ON dbo.TaxiTrips;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TaxiTrips_TripDistance' AND object_id = OBJECT_ID('dbo.TaxiTrips'))
    DROP INDEX IX_TaxiTrips_TripDistance ON dbo.TaxiTrips;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TaxiTrips_TripDurationSeconds' AND object_id = OBJECT_ID('dbo.TaxiTrips'))
    DROP INDEX IX_TaxiTrips_TripDurationSeconds ON dbo.TaxiTrips;
GO

CREATE INDEX IX_TaxiTrips_PULocationID
ON dbo.TaxiTrips (PULocationID)
INCLUDE (TipAmount);
GO

CREATE INDEX IX_TaxiTrips_TripDistance
ON dbo.TaxiTrips (TripDistance DESC)
INCLUDE (FareAmount, PickupDatetime, DropoffDatetime);
GO

CREATE INDEX IX_TaxiTrips_TripDurationSeconds
ON dbo.TaxiTrips (TripDurationSeconds DESC)
INCLUDE (PickupDatetime, DropoffDatetime);
GO
