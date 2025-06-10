namespace FFlow.Steps.DotNet;

public record DotnetTestResult
{
    public string Output { get; init; }
    public string Error { get; init; }
    public int ExitCode { get; init; }

    public int Passed { get; init; }
    public int Failed { get; init; }
    public int Skipped { get; init; }

    public bool Success => ExitCode == 0 && Failed == 0;
}

