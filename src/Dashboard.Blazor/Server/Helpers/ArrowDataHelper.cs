using Apache.Arrow;
using ParquetSharp;

namespace Dashboard.Blazor.Server.Helpers;

public static class ArrowDataHelper
{
    private static readonly string _parquetFileName = "C:\\Users\\Anatoly\\Documents\\GitHub\\radzen-blazor-dashboard\\src\\Dashboard.Blazor\\Client\\wwwroot\\sample-data\\fhvhv_tripdata_2023-08.parquet";

    private static readonly string[] fhvhv_tripdata_columns = new[]
    {
        "request_datetime"
        ,"on_scene_datetime"
        ,"pickup_datetime"
        ,"dropoff_datetime"
        ,"PULocationID"
        ,"DOLocationID"
        ,"trip_miles"
        ,"base_passenger_fare"
        ,"tolls"
        ,"bcf"
        ,"sales_tax"
        ,"congestion_surcharge"
        ,"airport_fee"
        ,"tips"
        ,"driver_pay"
    };

    public static IEnumerable<RecordBatch> ParquetToArrow(int batchnum = 0)
    {
        using var parquetReader = new ParquetFileReader(_parquetFileName);
        var df = parquetReader.ToDataFrame(columns: fhvhv_tripdata_columns, rowGroupIndices: new[] { batchnum });
        return df.ToArrowRecordBatches();
    }
}
