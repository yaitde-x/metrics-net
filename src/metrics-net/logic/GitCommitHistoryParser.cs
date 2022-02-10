namespace MetricsNet;

public class GitCommitHistoryParser
{
    public async Task<Commit[]> Parse(Stream? stream){
        
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using var reader = new StreamReader(stream, leaveOpen: true);

        var commits = new List<Commit>();
        var description = string.Empty;
        var author = string.Empty;
        var commitDate = default(DateTime);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;
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
        
        return commits.ToArray();
    }

    private bool IsSourceFileLine(string line)
    {
        return !(line.Length == 160 && line[1] != '\t' && DateTime.TryParse(line.Substring(150, 10), out var st));
    }
}
