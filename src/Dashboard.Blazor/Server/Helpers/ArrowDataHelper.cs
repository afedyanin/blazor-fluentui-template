using Apache.Arrow;
using ParquetSharp;

namespace Dashboard.Blazor.Server.Helpers;

public static class ArrowDataHelper
{
    private static readonly string _parquetFileName = "D:\\workspace\\blazor-dashboard\\src\\Dashboard.Blazor\\Client\\wwwroot\\sample-data\\fhvhv_tripdata_2023-09.parquet";

    private static readonly string[] fhvhv_tripdata_columns = new[]
    {
        "request_datetime"
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
    };

    public static IEnumerable<RecordBatch> ParquetToArrow(int batches = 1)
    {
        var groups = Enumerable.Range(0, batches).ToArray();
        using var parquetReader = new ParquetFileReader(_parquetFileName);
        var df = parquetReader.ToDataFrame(columns: fhvhv_tripdata_columns, rowGroupIndices: groups);
        return df.ToArrowRecordBatches();
    }
}
