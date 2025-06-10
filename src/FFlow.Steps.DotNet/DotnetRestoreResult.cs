namespace FFlow.Steps.DotNet;

public record DotnetRestoreResult(int ExitCode, string Output, string Error);
