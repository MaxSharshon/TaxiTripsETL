USE TaxiTripsDb;
GO

-- 1) PULocationID with highest average tip
SELECT TOP 1
    PULocationID,
    AVG(TipAmount) AS AvgTipAmount
FROM dbo.TaxiTrips
WHERE TipAmount IS NOT NULL
GROUP BY PULocationID
ORDER BY AVG(TipAmount) DESC;
GO

-- 2) Top 100 longest trips by distance
SELECT TOP 100
    PickupDatetime,
    DropoffDatetime,
    TripDistance,
    FareAmount,
    PULocationID,
    DOLocationID
FROM dbo.TaxiTrips
ORDER BY TripDistance DESC;
GO

-- 3) Top 100 longest trips by travel time
SELECT TOP 100
    PickupDatetime,
    DropoffDatetime,
    DATEDIFF(SECOND, PickupDatetime, DropoffDatetime) AS TravelSeconds,
    FareAmount,
    PULocationID,
    DOLocationID
FROM dbo.TaxiTrips
ORDER BY DATEDIFF(SECOND, PickupDatetime, DropoffDatetime) DESC;
GO

-- 4) Search by pickup location
SELECT TOP 100
    Id,
    PickupDatetime,
    DropoffDatetime,
    PassengerCount,
    TripDistance,
    FareAmount,
    TipAmount
FROM dbo.TaxiTrips
WHERE PULocationID = 238
ORDER BY PickupDatetime DESC;
GO
