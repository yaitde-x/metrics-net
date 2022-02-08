
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

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

public class GitHistoryParserTests
{
    [Fact]
    public void HappyPath()
    {

        var lines = File.ReadAllLines("/Users/sakamoto/Code/metrics-net/src/test/test-files/fusion-git-history.txt");
        var commits = new List<Commit>();
        var description = string.Empty;
        var author = string.Empty;
        var commitDate = default(DateTime);

        foreach (var line in lines)
        {
            if (line.Trim() == "")
            {
                continue;
            }
            else if (!IsSourceFileLine(line))
            {
                description = line.Substring(0, 100).Trim();
                author = line.Substring(100, 50).Trim();
                var commitDateBuf = line.Substring(150, 10);
                commitDate = DateTime.Parse(commitDateBuf);
            }
            else
            {
                var changeType = line.Substring(0, 1);
                var sourceFile = line.Substring(2);

                commits.Add(new Commit(commitDate, author, changeType, sourceFile));
            }
        }
    }

    private bool IsSourceFileLine(string line)
    {
        return !(line.Length == 160 && line[1] != '\t' && DateTime.TryParse(line.Substring(150, 10), out var st));
    }
}