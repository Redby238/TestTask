using Microsoft.EntityFrameworkCore;
using TestTask.DbSettings;

namespace TestTask.Services;

public class TaxiTripRepository(DbContextOptions<AppDbContext> options)
{
    public async Task SaveBatchAsync(List<TaxiTrip> batch)
    {
       
        await using var context = new AppDbContext(options);
        
        await context.TaxiTrips.AddRangeAsync(batch);
        await context.SaveChangesAsync();
    }
}