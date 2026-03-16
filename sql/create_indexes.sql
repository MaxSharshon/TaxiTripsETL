USE TaxiTripsDb;
GO

CREATE INDEX IX_TaxiTrips_PULocationID ON TaxiTrips(PULocationID);

CREATE INDEX IX_TaxiTrips_TripDistance ON TaxiTrips(TripDistance DESC);

CREATE INDEX IX_TaxiTrips_Time ON TaxiTrips(PickupDatetime, DropoffDatetime);