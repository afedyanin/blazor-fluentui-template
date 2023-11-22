using Apache.Arrow;
using Microsoft.Data.Analysis;

namespace Dashboard.Blazor.Shared.Tests;
public class ArrowIntegrationTests
{
    [Test]
    public void TestArrowIntegration()
    {
        RecordBatch originalBatch = new RecordBatch.Builder()
            .Append("Column1", false, col => col.Int32(array => array.AppendRange(Enumerable.Range(0, 10))))
            .Append("Column2", true, new Int32Array(
                valueBuffer: new ArrowBuffer.Builder<int>().AppendRange(Enumerable.Range(0, 10)).Build(),
                nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0xfd).Append(0xff).Build(),
                length: 10,
                nullCount: 1,
                offset: 0))
            .Append("Column3", true, new Int32Array(
                valueBuffer: new ArrowBuffer.Builder<int>().AppendRange(Enumerable.Range(0, 10)).Build(),
                nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0x00).Append(0x00).Build(),
                length: 10,
                nullCount: 10,
                offset: 0))
            .Append("NullableBooleanColumn", true, new BooleanArray(
                valueBuffer: new ArrowBuffer.Builder<byte>().Append(0xfd).Append(0xff).Build(),
                nullBitmapBuffer: new ArrowBuffer.Builder<byte>().Append(0xed).Append(0xff).Build(),
                length: 10,
                nullCount: 2,
                offset: 0))
            .Append("StringDataFrameColumn", false, new StringArray.Builder().AppendRange(Enumerable.Range(0, 10).Select(x => x.ToString())).Build())
            .Append("DoubleColumn", false, new DoubleArray.Builder().AppendRange(Enumerable.Repeat(1.0, 10)).Build())
            .Append("FloatColumn", false, new FloatArray.Builder().AppendRange(Enumerable.Repeat(1.0f, 10)).Build())
            .Append("ShortColumn", false, new Int16Array.Builder().AppendRange(Enumerable.Repeat((short)1, 10)).Build())
            .Append("LongColumn", false, new Int64Array.Builder().AppendRange(Enumerable.Repeat((long)1, 10)).Build())
            .Append("UIntColumn", false, new UInt32Array.Builder().AppendRange(Enumerable.Repeat((uint)1, 10)).Build())
            .Append("UShortColumn", false, new UInt16Array.Builder().AppendRange(Enumerable.Repeat((ushort)1, 10)).Build())
            .Append("ULongColumn", false, new UInt64Array.Builder().AppendRange(Enumerable.Repeat((ulong)1, 10)).Build())
            .Append("ByteColumn", false, new Int8Array.Builder().AppendRange(Enumerable.Repeat((sbyte)1, 10)).Build())
            .Append("UByteColumn", false, new UInt8Array.Builder().AppendRange(Enumerable.Repeat((byte)1, 10)).Build())
            .Append("Date64Column", false, new Date64Array.Builder().AppendRange(Enumerable.Repeat(DateTime.Now, 10)).Build())
            .Build();

        DataFrame df = DataFrame.FromArrowRecordBatch(originalBatch);
        IEnumerable<RecordBatch> recordBatches = df.ToArrowRecordBatches();

        var count = 0;
        foreach (RecordBatch batch in recordBatches)
        {
            Console.WriteLine($"Batch N={count++}");
        }
    }
}
