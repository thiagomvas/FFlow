using System.Text;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Executes the <c>dotnet clean</c> command for a specified project or solution.
/// Throws if the command fails.
/// </summary>
[StepName(".NET Clean")]
[StepTags("dotnet")]
[DotnetStep("projectOrSolution", "ProjectOrSolution")]
public class DotnetCleanStep : IFlowStep
{
    /// <summary>The project or solution to clean.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Configuration to clean (e.g., Debug, Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Target framework.</summary>
    public string? Framework { get; set; }

    /// <summary>Runtime identifier.</summary>
    public string? Runtime { get; set; }

    /// <summary>Suppress logo output.</summary>
    public bool NoLogo { get; set; }

    /// <summary>Artifacts directory path.</summary>
    public string? ArtifactsPath { get; set; }

    /// <summary>Output directory.</summary>
    public string? Output { get; set; }

    /// <summary>Verbosity level.</summary>
    public string? Verbosity { get; set; }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var command = BuildCommand();
        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
            throw new InvalidOperationException($"Dotnet clean failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
    }

    private string BuildCommand()
    {
        var sb = new StringBuilder("dotnet clean");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" \"{ProjectOrSolution}\"");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        if (!string.IsNullOrWhiteSpace(Framework)) sb.Append($" --framework {Framework}");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (!string.IsNullOrWhiteSpace(ArtifactsPath)) sb.Append($" --artifacts-path \"{ArtifactsPath}\"");
        if (!string.IsNullOrWhiteSpace(Output)) sb.Append($" --output \"{Output}\"");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");
        if (NoLogo) sb.Append(" --nologo");

        return sb.ToString();
    }
}
