
using System;

namespace MetricsNet;

public class Commit
{
    public Commit(DateTime commitDate, string author, string changeType, string sourceFile)
    {
        CommitDate = commitDate;
        Author = author;
        ChangeType = changeType;
        SourceFile = sourceFile;
    }

    public DateTime CommitDate { get; private set; }
    public string Author { get; private set; }
    public string ChangeType { get; private set; }
    public string SourceFile { get; private set; }

}
