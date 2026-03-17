using CsvHelper.Configuration;

namespace TaxiTripsETL.Models;

public sealed class TaxiTripMap : ClassMap<TaxiTrip>
{
    public TaxiTripMap()
    {
        Map(m => m.PickupDatetime).Name("tpep_pickup_datetime");
        Map(m => m.DropoffDatetime).Name("tpep_dropoff_datetime");
        Map(m => m.PassengerCount).Name("passenger_count");
        Map(m => m.TripDistance).Name("trip_distance");
        Map(m => m.StoreAndForwardFlag).Name("store_and_fwd_flag");
        Map(m => m.PickupLocationId).Name("PULocationID");
        Map(m => m.DropoffLocationId).Name("DOLocationID");
        Map(m => m.FareAmount).Name("fare_amount");
        Map(m => m.TipAmount).Name("tip_amount");
    }
}