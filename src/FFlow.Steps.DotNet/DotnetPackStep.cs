using System.Text;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// A workflow step that executes the <c>dotnet pack</c> command to generate NuGet packages
/// from a specified .NET project or solution.
/// </summary>
[StepName(".NET Pack")]
[StepTags("dotnet")]
public class DotnetPackStep : IFlowStep
{
    
    /// <summary>The project or solution file to pack.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Path to the directory where the package artifacts will be stored.</summary>
    public string? ArtifactsPath { get; set; }

    /// <summary>Build configuration to use (e.g., Debug or Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Forces all dependencies to be resolved and rebuilt.</summary>
    public bool Force { get; set; }

    /// <summary>Includes the source files in the package.</summary>
    public bool IncludeSource { get; set; }

    /// <summary>Includes the debug symbols in the package.</summary>
    public bool IncludeSymbols { get; set; }

    /// <summary>Allows the command to stop and wait for user input or authentication.</summary>
    public bool Interactive { get; set; }

    /// <summary>Skips building the project before packing.</summary>
    public bool NoBuild { get; set; }

    /// <summary>Skips packing project-to-project dependencies.</summary>
    public bool NoDependencies { get; set; }

    /// <summary>Skips the automatic restore of the project before packing.</summary>
    public bool NoRestore { get; set; }

    /// <summary>Hides the startup banner and copyright message.</summary>
    public bool NoLogo { get; set; }

    /// <summary>Output directory for the generated package.</summary>
    public string? Output { get; set; }

    /// <summary>Target runtime identifier (e.g., win-x64, linux-x64).</summary>
    public string? Runtime { get; set; }

    /// <summary>Marks the package as serviceable.</summary>
    public bool Serviceable { get; set; }

    /// <summary>Toolset logging level: auto, on, or off.</summary>
    public string? TL { get; set; }

    /// <summary>Controls the verbosity of the output (quiet, minimal, normal, detailed, diagnostic).</summary>
    public string? Verbosity { get; set; }

    /// <summary>Defines the version suffix to use.</summary>
    public string? VersionSuffix { get; set; }

    /// <summary>
    /// The result of the <c>dotnet pack</c> command execution.
    /// </summary>
    public DotnetPackResult? Result { get; private set; }
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the pack step.");
        }

        var command = BuildCommand();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet pack failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        Result = new DotnetPackResult(exitCode, output, error);
        context.SetOutputFor<DotnetPackStep, DotnetPackResult>(Result);
        
    }

    private string BuildCommand()
    {
        var sb = new StringBuilder("dotnet pack");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" {ProjectOrSolution}");
        if (!string.IsNullOrWhiteSpace(ArtifactsPath)) sb.Append($" --artifacts-path \"{ArtifactsPath}\"");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        if (Force) sb.Append(" --force");
        if (IncludeSource) sb.Append(" --include-source");
        if (IncludeSymbols) sb.Append(" --include-symbols");
        if (Interactive) sb.Append(" --interactive");
        if (NoBuild) sb.Append(" --no-build");
        if (NoDependencies) sb.Append(" --no-dependencies");
        if (NoRestore) sb.Append(" --no-restore");
        if (NoLogo) sb.Append(" --nologo");
        if (!string.IsNullOrWhiteSpace(Output)) sb.Append($" --output \"{Output}\"");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (Serviceable) sb.Append(" --serviceable");
        if (!string.IsNullOrWhiteSpace(TL)) sb.Append($" --tl:{TL}");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");
        if (!string.IsNullOrWhiteSpace(VersionSuffix)) sb.Append($" --version-suffix {VersionSuffix}");

        return sb.ToString();
    }
}