namespace TaxiTripsETL.Models;

public class TaxiTrip
{
    public DateTime PickupDatetime { get; set; }

    public DateTime DropoffDatetime { get; set; }

    public int? PassengerCount { get; set; }

    public double? TripDistance { get; set; }

    public string StoreAndForwardFlag { get; set; }

    public int? PickupLocationId { get; set; }

    public int? DropoffLocationId { get; set; }

    public decimal? FareAmount { get; set; }

    public decimal? TipAmount { get; set; }
}