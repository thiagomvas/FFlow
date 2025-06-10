namespace FFlow.Steps.DotNet;

/// <summary>
/// Represents the result of executing a <c>dotnet test</c> operation,
/// including the exit code, standard output, error output and test results.
/// </summary>
public record DotnetTestResult
{
    /// <summary>
    /// Gets the standard output produced by the <c>dotnet test</c> operation.
    /// </summary>
    public string Output { get; init; }

    /// <summary>
    /// Gets the error output produced by the <c>dotnet test</c> operation.
    /// </summary>
    public string Error { get; init; }

    /// <summary>
    /// Gets the exit code returned by the <c>dotnet test</c> operation.
    /// </summary>
    public int ExitCode { get; init; }

    /// <summary>
    /// Gets the number of tests that passed during the <c>dotnet test</c> operation.
    /// </summary>
    public int Passed { get; init; }

    /// <summary>
    /// Gets the number of tests that failed during the <c>dotnet test</c> operation.
    /// </summary>
    public int Failed { get; init; }

    /// <summary>
    /// Gets the number of tests that were skipped during the <c>dotnet test</c> operation.
    /// </summary>
    public int Skipped { get; init; }

    /// <summary>
    /// Gets a value indicating whether the <c>dotnet test</c> operation was successful.
    /// Success is determined by an exit code of 0 and no failed tests.
    /// </summary>
    public bool Success => ExitCode == 0 && Failed == 0;
}