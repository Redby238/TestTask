namespace TestTask.DbSettings;

public class TaxiTrip
{
    public int Id { get; set; }
    
    public DateTime PickupDatetimeUTC { get; set; }
    public DateTime DropoffDatetimeUTC { get; set; }
    
    public int? PassengerCount { get; set; }
    public decimal? TripDistance { get; set; }
    public string? StoreAndFwdFlag { get; set; } 
    public int? PULocationID { get; set; }
    public int? DOLocationID { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? TipAmount { get; set; }

    public int? TripDurationSeconds { get; set; } 
}