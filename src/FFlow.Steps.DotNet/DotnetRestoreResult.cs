namespace FFlow.Steps.DotNet;

/// <summary>
/// Represents the result of executing a <c>dotnet restore</c> operation,
/// including the exit code, standard output, and error output.
/// </summary>
/// <param name="ExitCode">The exit code returned by the <c>dotnet publish</c> command. A value of 0 indicates success.</param>
/// <param name="Output">The standard output produced during the publish operation.</param>
/// <param name="Error">The standard error output produced during the publish operation.</param>
public record DotnetRestoreResult(int ExitCode, string Output, string Error);