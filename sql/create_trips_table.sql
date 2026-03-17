USE TaxiTripsDb;
GO

SET QUOTED_IDENTIFIER ON;
GO

SET ANSI_NULLS ON;
GO

IF OBJECT_ID('dbo.TaxiTrips', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.TaxiTrips;
END
GO

CREATE TABLE dbo.TaxiTrips
(
    Id BIGINT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    PickupDatetime DATETIME2(0) NOT NULL,
    DropoffDatetime DATETIME2(0) NOT NULL,
    PassengerCount INT NULL,
    TripDistance DECIMAL(9, 3) NULL,
    StoreAndFwdFlag VARCHAR(3) NULL,
    PULocationID INT NULL,
    DOLocationID INT NULL,
    FareAmount DECIMAL(10, 2) NULL,
    TipAmount DECIMAL(10, 2) NULL,
    TripDurationSeconds AS DATEDIFF(SECOND, PickupDatetime, DropoffDatetime) PERSISTED
);
GO

ALTER TABLE dbo.TaxiTrips
ADD CONSTRAINT CK_TaxiTrips_StoreAndFwdFlag
CHECK (StoreAndFwdFlag IN ('Yes', 'No') OR StoreAndFwdFlag IS NULL);
GO
