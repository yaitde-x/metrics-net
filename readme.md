
# metrics-net

This is a research project centered around analyzing code metrics, git history and stack traces

metrics-net is a command line utility for parsing the xml output produced by running the microsoft There are three related but separate threads of work here.

__Note: --format was never implemented so the ouput is always JSON__

## Code Metrics

https://docs.microsoft.com/en-us/visualstudio/code-quality/how-to-generate-code-metrics-data?view=vs-2022

```
metrics /solution:source/fusion.sln /out:c:\temp\fusion-metrics-2022-03-31.xml
```

This command produces an xml file. You can then feed this xml file into metrics-net to

__Transform to JSON__
```
metrics-net.exe parse --input c:\temp\fusion-metrics-2022-03-31.xml --output c:\temp\fusion-metrics-2022-03-31.json --type object
metrics-net.exe parse --input c:\temp\fusion-metrics-2022-03-31.xml --output c:\temp\fusion-metrics-2022-03-31.jsohn --type record
```

### Snippet with output type = object
```json
{
  "Targets": [
    {
      "Name": "Karmak.Services.PrintManager.Server.csproj",
      "Metrics": null,
      "Assemblies": [
        {
          "Name": "Karmak.Services.PrintManager.Server, Version=3.0.0.0, Culture=neutral, PublicKeyToken=4340177d7467d9dc",
          "Metrics": {
            "MaintainabilityIndex": 73,
            "CyclomaticComplexity": 153,
            "ClassCoupling": 77,
            "DepthOfInheritance": 3,
            "SourceLines": 1123,
            "ExecutableLines": 376
          },
          "Namespaces": [
            {
              "Name": "Karmak.Services.PrintManager.Server",
              "Metrics": {
                "MaintainabilityIndex": 73,
                "CyclomaticComplexity": 153,
                "ClassCoupling": 77,
                "DepthOfInheritance": 3,
                "SourceLines": 1123,
                "ExecutableLines": 376
              },
              "Types": [
                {
                  "Name": "MgrPrintManager",
                  "Metrics": {
                    "MaintainabilityIndex": 68,
                    "CyclomaticComplexity": 51,
                    "ClassCoupling": 41,
                    "DepthOfInheritance": 3,
                    "SourceLines": 416,
                    "ExecutableLines": 146
                  },
...                  
```

### Snippet with type = record
```json
[
  {
    "Period": "2022-03-31T11:08:19.6635002-05:00",
    "Target": "Karmak.Services.PrintManager.Server.csproj",
    "Assembly": "Karmak.Services.PrintManager.Server, Version=3.0.0.0, Culture=neutral, PublicKeyToken=4340177d7467d9dc",
    "Namespace": "Karmak.Services.PrintManager.Server",
    "Type": "MgrPrintManager",
    "Signature": "",
    "Language": 1,
    "Raw": "MgrPrintManager.MgrPrintManager()",
    "MemberName": "",
    "ReturnType": "",
    "MaintainabilityIndex": 100,
    "CyclomaticComplexity": 1,
    "ClassCoupling": 1,
    "DepthOfInheritance": 0,
    "LinesOfCode": 4
  },
...
]
```

__You can also write directly to a sql table__
```
metrics-net.exe parse --input c:\temp\fusion-metrics-2022-03-31.xml --sql "Server=localhost;Database=metrics;Trusted_Connection=True;" --table rawMetrics
```

### Table Schema
```sql
CREATE TABLE [dbo].[rawMetrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [datetime] NULL,
	[Module] [varchar](4096) NULL,
	[Namespace] [varchar](4096) NULL,
	[Type] [varchar](4096) NULL,
	[Member] [varchar](4096) NULL,
	[Raw] [varchar](4096) NULL,
	[MemberName] [varchar](500) NULL,
	[ReturnType] [varchar](100) NULL,
	[Language] [varchar](20) NULL,
	[MaintainabilityIndex] [int] NULL,
	[CyclomaticComplexity] [int] NULL,
	[ClassCoupling] [int] NULL,
	[DepthOfInheritance] [int] NULL,
	[LinesOfCode] [int] NULL
) ON [PRIMARY]
```

## Git History Scrape

This research was not complete. The concept is to mine git history for file changes. The concept behind the research 

```
git log --name-status  --pretty="format:%<(100)%f%<(50)%an%cs" >  /c/temp/fusion-git-history.txt
```

This command produces output like so:
```
Merged-PR-5253-simulate-responses-in-fitnesse-for-ups-communications                                Chris Yeager                                      2022-03-30
M	FitNesseRoot/FrontPage/ProfitMaster/InventorY/UpsShipping/FunctionalArea/UpsShipping/content.txt
M	FitNesseRoot/FrontPage/ProfitMaster/InventorY/UpsShipping/FunctionalArea/UpsShippingAndHandlingCombined/content.txt
M	FitNesseRoot/FrontPage/ProfitMaster/InventorY/UpsShipping/FunctionalArea/UpsShippingAndHandlingCombined2/content.txt
M	FitNesseRoot/FrontPage/ProfitMaster/InventorY/UpsShipping/FunctionalArea/UpsShippingHistory/content.txt
M	Source/Inventory/Karmak.BusinessServices.PM.Inventory.Facade/Shipping/MgrUpsShipping.vb
M	Source/Inventory/Karmak.BusinessServices.PM.Inventory.FacadeInterfaces/Shipping/KarmakUPSFaultException.vb
M	Source/Karmak.Team1/Karmak.Team1/PM30 Fixtures/Base/PMSession.vb
M	Source/Karmak.Team1/Karmak.Team1/PM30 Fixtures/InspectDataTable.vb

Merged-PR-5190-refresh-core3-from-devint                                                            Eric Oliver                                       2022-03-25
refresh-core3-from-devint                                                                           Eric Oliver                                       2022-03-25
Merged-PR-5149-VSTS-120665-Metadata-cleanup-for-new-UAP-objects                                     James Zeitler                                     2022-03-23
M	Source/PM_2_0_DB/PM_2_0/Scripts/Post-Deployment/FRWMetadataColumn.sql
M	Source/PM_2_0_DB/PM_2_0/Scripts/Post-Deployment/FRWMetadataJoin.sql
M	Source/PM_2_0_DB/PM_2_0/Scripts/Post-Deployment/FRWMetadataTable.sql

Merged-PR-5138-Updated-names-of-views                                                               Spencer Smith                                     2022-03-23
M	Source/PM_2_0_DB/PM_2_0/PM_2_0_DB.sqlproj
R100	Source/PM_2_0_DB/PM_2_0/Schema Objects/Views/dbo.vwIN_SSR_PartsOrderConsignmentPriceUpdates.sql	Source/PM_2_0_DB/PM_2_0/Schema Objects/Views/dbo.vwIN_SSR_PartsOrderConsignmentPriceUpdates.view.sql
R100	Source/PM_2_0_DB/PM_2_0/Schema Objects/Views/dbo.vwIN_SSR_PartsOrderQuotePriceUpdates.sql	Source/PM_2_0_DB/PM_2_0/Schema Objects/Views/dbo.vwIN_SSR_PartsOrderQuotePriceUpdates.view.sql
```

## StackTraceParser

This is incomplete research around parsing stack trace for call dependencies. Where I left off with this was writing tests to drive development of the StackTraceParser. The stack command is not yet implemented. The concept is to consume a feed to stack traces, parsing the Originator (entry point), the Offender (he who threw the exception), and create a call chain of dependencies. This would feed a database to help develop and picture in a large codebase of what coupling exists between modules, etc.



```csharp
public class StackTrace
{
    public StackTrace(MemberSignature originator, MemberSignature offender, StackDependency[] callChain)
    {
        Originator = originator;
        Offender = offender;
        CallChain = callChain;
    }

    public MemberSignature Originator { get; private set; }

    public MemberSignature Offender { get; private set; }
    public StackDependency[] CallChain { get; private set; }
}
```