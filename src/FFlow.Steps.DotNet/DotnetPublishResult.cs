namespace FFlow.Steps.DotNet;

public record DotnetPublishResult(int ExitCode, string Output, string Error);