using Apache.Arrow;
using Apache.Arrow.Ipc;
using ParquetSharp;
using ParquetSharp.Arrow;

namespace Dashboard.Blazor.Shared.Tests;

public class ArrowFileTests
{
    private static readonly string _arrowFileName = "C:\\Users\\Anatoly\\Documents\\GitHub\\radzen-blazor-dashboard\\src\\Dashboard.Blazor\\Client\\wwwroot\\sample-data\\weather.json.arrow";
    private static readonly string _parquetFileName = "C:\\Users\\Anatoly\\Documents\\GitHub\\radzen-blazor-dashboard\\src\\Dashboard.Blazor\\Client\\wwwroot\\sample-data\\fhvhv_tripdata_2023-08.parquet";

    [Test]
    public async Task CanReadArrowFile()
    {
        using var stream = File.OpenRead(_arrowFileName);
        using var reader = new ArrowFileReader(stream);
        var recordBatch = await reader.ReadNextRecordBatchAsync();
        Console.WriteLine("Read record batch with {0} column(s)", recordBatch.ColumnCount);
        Assert.Pass();
    }

    [Test]
    public async Task CanReadParquetAsArrow()
    {
        using var fileReader = new FileReader(_parquetFileName);
        using var batchReader = fileReader.GetRecordBatchReader();

        RecordBatch batch;
        var count = 0;
        while ((batch = await batchReader.ReadNextRecordBatchAsync()) != null)
        {
            using (batch)
            {
                Console.WriteLine($"Batch #{count++}");
            }
        }
    }

    [Test]
    public async Task CanConvertDataFrameIntoArrow()
    {
        using var parquetReader = new ParquetFileReader(_parquetFileName);
        var df = parquetReader.ToDataFrame(rowGroupIndices: new[] { 0 });
        parquetReader.Close();

        var batches = df.ToArrowRecordBatches();

        using var stream = new MemoryStream();
        ArrowStreamWriter? writer = null;

        foreach (var recordBatch in batches)
        {
            writer ??= new ArrowStreamWriter(stream, recordBatch.Schema);
            await writer.WriteRecordBatchAsync(recordBatch);
        }

        await writer!.WriteEndAsync();
        var bytes = stream.ToArray();

        Assert.That(bytes, Is.Not.Empty);
        Console.WriteLine($"bytes={bytes.Length}");
    }

    [Test]
    public void CanIterateRecordBatches()
    {
        using var parquetReader = new ParquetFileReader(_parquetFileName);

        var df = parquetReader.ToDataFrame(
            columns: new[]
            {
                 "hvfhs_license_num"
                ,"dispatching_base_num"
                ,"originating_base_num"
                ,"request_datetime"
                ,"on_scene_datetime"
                ,"pickup_datetime"
                ,"dropoff_datetime"
                ,"PULocationID"
                ,"DOLocationID"
                ,"trip_miles"
                ,"trip_time"
                ,"base_passenger_fare"
                ,"tolls"
                ,"bcf"
                ,"sales_tax"
                ,"congestion_surcharge"
                ,"airport_fee"
                ,"tips"
                ,"driver_pay"
                ,"shared_request_flag"
                ,"shared_match_flag"
                ,"access_a_ride_flag"
                ,"wav_request_flag"
                ,"wav_match_flag"
            },
            rowGroupIndices: new[] { 0 });

        parquetReader.Close();

        foreach(var col in df.Columns)
        {
            Console.WriteLine($"Col: Name={col.Name} DataType={col.DataType} ColumnType={col.GetType().Name}");
        }

        var count = 0;
        foreach (var recordBatch in df.ToArrowRecordBatches())
        {
            // var schema = recordBatch.Schema;
            // Console.WriteLine($"schema: TotalFields={schema.Fields.Count} HasMetadata={schema.HasMetadata}");
            Console.WriteLine($"Batch N={count++}");
        }
    }
}
