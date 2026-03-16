USE TaxiTripsDb;
GO

CREATE TABLE TaxiTrips
(
    Id INT IDENTITY PRIMARY KEY,

    PickupDatetime DATETIME2 NOT NULL,
    DropoffDatetime DATETIME2 NOT NULL,

    PassengerCount INT,

    TripDistance FLOAT,

    StoreAndFwdFlag NVARCHAR(3),

    PULocationID INT,
    DOLocationID INT,

    FareAmount DECIMAL(10,2),
    
    TipAmount DECIMAL(10,2)
);