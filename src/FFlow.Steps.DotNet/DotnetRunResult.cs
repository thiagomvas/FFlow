namespace FFlow.Steps.DotNet;

public record DotnetRunResult(int ExitCode, string Output, string Error);