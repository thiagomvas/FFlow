namespace FFlow.Steps.DotNet;

public record DotnetBuildResult(int ExitCode, string Output, string Error);
