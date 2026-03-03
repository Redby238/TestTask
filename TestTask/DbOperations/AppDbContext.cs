using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace TestTask.DbSettings;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<TaxiTrip> TaxiTrips { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaxiTrip>(entity =>
        {
            entity.ToTable("TaxiTrips");
            entity.HasKey(e => e.Id);

            
            entity.Property(e => e.StoreAndFwdFlag).HasMaxLength(3);
            entity.Property(e => e.TripDistance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FareAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipAmount).HasColumnType("decimal(10, 2)");

            
            entity.Property(e => e.TripDurationSeconds)
                .HasComputedColumnSql("DATEDIFF(SECOND, PickupDatetimeUTC, DropoffDatetimeUTC)", stored: true);

            

            
            entity.HasIndex(e => e.PULocationID)
                .IncludeProperties(e => e.TipAmount)
                .HasDatabaseName("IX_TaxiTrips_PULocationID_Includes");

            
            entity.HasIndex(e => e.TripDistance)
                .IsDescending()
                .HasDatabaseName("IX_TaxiTrips_TripDistance");

            
            entity.HasIndex(e => e.TripDurationSeconds)
                .IsDescending()
                .HasDatabaseName("IX_TaxiTrips_TripDuration");
        });
    }
}