
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsNet;

public class GitHistoryParserTests
{
    [Fact]
    public async void HappyPath()
    {

        using var stream = File.OpenRead("/Users/sakamoto/Code/metrics-net/src/test/test-files/fusion-git-history.txt");
        var parser = new GitCommitHistoryParser();
        var commits = await parser.Parse(stream);
        
        Assert.NotNull(stream);
    }

    private bool IsSourceFileLine(string line)
    {
        return !(line.Length == 160 && line[1] != '\t' && DateTime.TryParse(line.Substring(150, 10), out var st));
    }
}