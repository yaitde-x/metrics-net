
using System.Text;

namespace MetricsNet;

public class CsvMetricWriter
{

    public async Task Write(Stream stream, CodeMetricRecord[] records)
    {

        using var writer = new StreamWriter(stream, leaveOpen: true);

        foreach (var record in records)
        {
            var buf = ToCsvRecord(record);
            await writer.WriteLineAsync(buf);
        }
    }

    private string GetHeader() {
        return "Period,Target,Assembly,Namespace,Type,member,MaintainabilityIndex,CyclomaticComplexity,ClassCoupling,DepthOfInheritance,LinesOfCode";
    }
    private string ToCsvRecord(CodeMetricRecord record)
    {
        return $"\"{record.Period}\",\"{record.Target}\",\"{record.Assembly}\",\"{record.Namespace}\",\"{record.Type}\",\"{record.Member}\",{record.MaintainabilityIndex},{record.CyclomaticComplexity},{record.ClassCoupling},{record.DepthOfInheritance},{record.LinesOfCode}";
    }
}