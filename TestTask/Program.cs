using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestTask.DbSettings;
using TestTask.Services;

Console.WriteLine("=== Running ETL Pipeline TaxiTrips ===");

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();
string? connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Connection string isn't found!";

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer(connectionString);

string inputCsvPath = "sample-cab-data.csv"; 
string duplicatesCsvPath = "duplicates.csv";

if (!File.Exists(inputCsvPath))
{
    Console.WriteLine($"Error: {inputCsvPath} is not found in the current directory.");
    return;
}


var extractor = new CsvExtractor();
var repository = new TaxiTripRepository(optionsBuilder.Options); 
var pipeline = new EtlPipeline(extractor, repository);

var stopwatch = Stopwatch.StartNew();

try
{
    
    await pipeline.RunAsync(inputCsvPath, duplicatesCsvPath);
}
catch (Exception ex)
{
    Console.WriteLine($"\nКритическая ошибка во время выполнения: {ex.Message}");
}
finally
{
    stopwatch.Stop();
    Console.WriteLine($"\nОбщее время выполнения: {stopwatch.Elapsed.TotalSeconds:F2} секунд.");
}