
using System.CommandLine;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using FastMember;
using Newtonsoft.Json;

namespace MetricsNet;

public class ParseCommand : Command
{
    public ParseCommand()
        : base("parse", "parse and xml file")
    {
        var inputFileOption = new Option<string>(new string[] { "--input", "-i" }, "path to input file") { IsRequired = true };
        var outputFileOption = new Option<string?>(new string[] { "--output", "-o" }, () => "", "path to output file");
        var outputTypeOption = new Option<string>(new string[] { "--type", "-t" }, () => "object", "output type [object|record], default=object");
        var formatOption = new Option<string>(new string[] { "--format", "-f" }, () => "json", "output type [json|csv], default=json");
        var sqlOption = new Option<string?>(new string[] { "--sql", "-s" }, () => "", "writes to sql record a connection string");
        var tableOption = new Option<string?>(new string[] { "--table", "-b" }, () => "", "this option is required if --sql is used");

        this.AddOption(inputFileOption);
        this.AddOption(outputFileOption);
        this.AddOption(outputTypeOption);
        this.AddOption(formatOption);
        this.AddOption(sqlOption);
        this.AddOption(tableOption);

        this.SetHandler<string, string?, string, string, string?, string?>(Handle, inputFileOption, outputFileOption,
                                                                            outputTypeOption, formatOption, sqlOption, tableOption);
    }

    public void Handle(string inputFile, string? outputFile, string outputType, string format, string? sqlConnectionString, string? tableName)
    {
        using var instream = File.OpenRead(inputFile.Trim());
        var parser = new XmlMetricsReportParser();
        var codeReport = parser.Parse(instream);

        if (!string.IsNullOrEmpty(sqlConnectionString) && string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentException("to use the sql option, need to pass --table as well");
        }

        if (!string.IsNullOrEmpty(outputFile))
        {
            using var outStream = File.OpenWrite(outputFile);
            using var writer = new StreamWriter(outStream);

            if (outputType.ToLower().Equals("object"))
            {
                writer.Write(JsonConvert.SerializeObject(codeReport, Formatting.Indented));
            }
            else if (outputType.ToLower().Equals("record"))
            {
                var transformer = new MetricRecordTransformer();
                var records = transformer.Transform(codeReport);

                writer.Write(JsonConvert.SerializeObject(records, Formatting.Indented));
            }
        }

        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            var conn = new SqlConnection(sqlConnectionString);
            var loader = new SqlBulkCopy(conn);

            conn.Open();

            var transformer = new MetricRecordTransformer();
            var records = transformer.Transform(codeReport);

/*
Period = period;
Assembly = assembly;
Target = target;
Namespace = ns;
Type = type;
Signature = simplified;  
MemberName = memberName;
Raw = raw;
ReturnType = returnType;
Language = language;
*/
            var timer = new Stopwatch();
            timer.Start();

            using (var bcp = new SqlBulkCopy(conn))
            using (var reader = ObjectReader
                                    .Create(records, 
                                    "Period", "Assembly", "Target", "Namespace", "Type", "Signature", "MemberName", 
                                    "Raw", "ReturnType", "Language", "MaintainabilityIndex", "CyclomaticComplexity",
                                    "ClassCoupling", "DepthOfInheritance", "LinesOfCode"))
            {
                bcp.DestinationTableName = "[raw-metrics]";

                bcp.ColumnMappings.Add("Period", "Period");
                bcp.ColumnMappings.Add("Assembly", "Module");
                bcp.ColumnMappings.Add("Namespace", "Namespace");
                bcp.ColumnMappings.Add("Type", "Type");
                bcp.ColumnMappings.Add("Signature", "Member");
                bcp.ColumnMappings.Add("MemberName", "MemberName");
                bcp.ColumnMappings.Add("Raw", "Raw");
                bcp.ColumnMappings.Add("ReturnType", "ReturnType");
                bcp.ColumnMappings.Add("Language", "Language");
                bcp.ColumnMappings.Add("MaintainabilityIndex", "MaintainabilityIndex");
                bcp.ColumnMappings.Add("CyclomaticComplexity", "CyclomaticComplexity");
                bcp.ColumnMappings.Add("ClassCoupling", "ClassCoupling");
                bcp.ColumnMappings.Add("DepthOfInheritance", "DepthOfInheritance");
                bcp.ColumnMappings.Add("LinesOfCode", "LinesOfCode");


                bcp.WriteToServer(reader);
            }

            timer.Stop();
            Console.WriteLine($"Time to load: {timer.ElapsedMilliseconds}");

            // loader.WriteToServerAsync()
            // foreach (var record in records.Take(100))
            // {
            //     var command = CreateInsertCommand(conn, record);
            //     var recordsAffected = command.ExecuteNonQuery();
            // }

        }
    }

    private IDbCommand CreateInsertCommand(IDbConnection conn, CodeMetricRecord record)
    {
        var command = conn.CreateCommand();

        command.CommandText = GetInsertTemplate();
        command.CommandType = CommandType.Text;

        command.Parameters.Add(SqlUtilities.CreateParameter(command, "period", record.Period));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "module", record.Assembly));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "namespace", record.Namespace));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "type", record.Type));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "signature", record.Signature));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "maintainabilityIndex", record.MaintainabilityIndex));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "cyclomaticComplexity", record.CyclomaticComplexity));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "classCoupling", record.ClassCoupling));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "depthOfInheritance", record.DepthOfInheritance));
        command.Parameters.Add(SqlUtilities.CreateParameter(command, "linesOfCode", record.LinesOfCode));

        return command;
    }

    private string GetInsertTemplate()
    {
        var sb = new StringBuilder();

        sb.AppendLine("INSERT INTO [dbo].[raw-metrics]");
        sb.AppendLine("           ([Period]");
        sb.AppendLine("           ,[Module]");
        sb.AppendLine("           ,[Namespace]");
        sb.AppendLine("           ,[Type]");
        sb.AppendLine("           ,[Signature]");
        sb.AppendLine("           ,[MemberName]");
        sb.AppendLine("           ,[Raw]");
        sb.AppendLine("           ,[ReturnType]");
        sb.AppendLine("           ,[MaintainabilityIndex]");
        sb.AppendLine("           ,[CyclomaticComplexity]");
        sb.AppendLine("           ,[ClassCoupling]");
        sb.AppendLine("           ,[DepthOfInheritance]");
        sb.AppendLine("           ,[LinesOfCode])");
        sb.AppendLine("     VALUES");
        sb.AppendLine("           (@period");
        sb.AppendLine("           ,@module");
        sb.AppendLine("           ,@namespace");
        sb.AppendLine("           ,@type");
        sb.AppendLine("           ,@signature");
        sb.AppendLine("           ,@memberName");
        sb.AppendLine("           ,@raw");
        sb.AppendLine("           ,@returnType");
        sb.AppendLine("           ,@maintainabilityIndex");
        sb.AppendLine("           ,@cyclomaticComplexity");
        sb.AppendLine("           ,@classCoupling");
        sb.AppendLine("           ,@depthOfInheritance");
        sb.AppendLine("           ,@linesOfCode)");

        return sb.ToString();
    }

    public async Task HandleAsync(string inputFile, string? outputFile, string outputType, string format, string? sqlConnectionString, string? tableName)
    {
        using var instream = File.OpenRead(inputFile.Trim());
        var parser = new XmlMetricsReportParser();
        var codeReport = parser.Parse(instream);

        if (!string.IsNullOrEmpty(sqlConnectionString) && string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentException("to use the sql option, need to pass --table as well");
        }

        if (!string.IsNullOrEmpty(outputFile))
        {
            using var outStream = File.OpenWrite(outputFile);
            using var writer = new StreamWriter(outStream);

            if (outputType.ToLower().Equals("object"))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(codeReport, Formatting.Indented));
            }
            else if (outputType.ToLower().Equals("record"))
            {
                var transformer = new MetricRecordTransformer();
                var records = transformer.Transform(codeReport);

                await writer.WriteAsync(JsonConvert.SerializeObject(records, Formatting.Indented));
            }
        }

        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            var conn = new SqlConnection(sqlConnectionString);
            await conn.OpenAsync();

            var transformer = new MetricRecordTransformer();
            var records = transformer.Transform(codeReport);

            foreach (var record in records)
            {
                var command = CreateInsertCommand(conn, record);
                var recordsAffected = command.ExecuteNonQuery();
            }

        }
    }
}