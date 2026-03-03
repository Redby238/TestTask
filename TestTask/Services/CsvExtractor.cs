using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TestTask.Contracts;
using TestTask.DbSettings;

namespace TestTask.Services;

public class CsvExtractor
{
    public async IAsyncEnumerable<TaxiTrip> ExtractTripsAsync(string inputCsvPath, string duplicatesCsvPath)
    {
        var seenRecords = new HashSet<(DateTime, DateTime, int)>();
        var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null,
        };

        using var reader = new StreamReader(inputCsvPath);
        using var csv = new CsvReader(reader, csvConfig);

        using var duplicateWriter = new StreamWriter(duplicatesCsvPath);
        using var duplicateCsv = new CsvWriter(duplicateWriter, csvConfig);

        duplicateCsv.WriteHeader<TaxiTripCsvRecord>();
        await duplicateCsv.NextRecordAsync();

        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<TaxiTripCsvRecord>();
            if (record == null) continue;

            var duplicateKey = (record.PickupDatetime, record.DropoffDatetime, record.PassengerCount ?? 0);
            if (!seenRecords.Add(duplicateKey))
            {
                duplicateCsv.WriteRecord(record);
                await duplicateCsv.NextRecordAsync();
                continue; // Пропускаем дубликат
            }

            string? mappedFlag = record.StoreAndFwdFlag?.ToUpper() switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => record.StoreAndFwdFlag
            };

            yield return new TaxiTrip
            {
                PickupDatetimeUTC = TimeZoneInfo.ConvertTimeToUtc(record.PickupDatetime, estZone),
                DropoffDatetimeUTC = TimeZoneInfo.ConvertTimeToUtc(record.DropoffDatetime, estZone),
                PassengerCount = record.PassengerCount,
                TripDistance = record.TripDistance,
                StoreAndFwdFlag = mappedFlag,
                PULocationID = record.PULocationID,
                DOLocationID = record.DOLocationID,
                FareAmount = record.FareAmount,
                TipAmount = record.TipAmount
            };
        }
    }
}