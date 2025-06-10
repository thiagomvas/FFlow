namespace FFlow.Steps.DotNet;

/// <summary>
/// Represents the result of executing a <c>dotnet build</c> operation,
/// including the process exit code, standard output, and error output.
/// </summary>
/// <param name="ExitCode">The exit code returned by the <c>dotnet build</c> command. A value of 0 typically indicates success.</param>
/// <param name="Output">The standard output produced during the build operation.</param>
/// <param name="Error">The standard error output produced during the build operation.</param>
public record DotnetBuildResult(int ExitCode, string Output, string Error);

