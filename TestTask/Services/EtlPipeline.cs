using TestTask.DbSettings;

namespace TestTask.Services;

public class EtlPipeline(CsvExtractor extractor, TaxiTripRepository repository)
{
    private const int BatchSize = 10000;

    public async Task RunAsync(string inputCsvPath, string duplicatesCsvPath)
    {
        Console.WriteLine("Starting processing ");
        
        var batch = new List<TaxiTrip>(BatchSize);
        int totalProcessed = 0;

        
        await foreach (var trip in extractor.ExtractTripsAsync(inputCsvPath, duplicatesCsvPath))
        {
            batch.Add(trip);
            totalProcessed++;

            if (batch.Count >= BatchSize)
            {
                await repository.SaveBatchAsync(batch);
                Console.WriteLine($"Inserted {totalProcessed} rows");
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            await repository.SaveBatchAsync(batch);
            Console.WriteLine($"Saved in DB {totalProcessed} rows");
        }

        Console.WriteLine($"\n Succesfully!  {totalProcessed} records.");
    }
}