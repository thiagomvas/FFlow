using System.Text;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// A workflow step that executes the <c>dotnet build</c> command to build a .NET project or solution.
/// </summary>
[StepName(".NET Build")]
[StepTags("dotnet")]
[DotnetStep("projectOrSolution", "ProjectOrSolution")]
public class DotnetBuildStep : IFlowStep
{
    
    /// <summary>The project or solution file to build.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Specifies the target architecture (e.g., x64, x86, arm).</summary>
    public string? Architecture { get; set; }

    /// <summary>Path to the directory where artifacts will be stored.</summary>
    public string? ArtifactsPath { get; set; }

    /// <summary>Build configuration (e.g., Debug or Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Target framework to build for (e.g., net7.0).</summary>
    public string? Framework { get; set; }

    /// <summary>Disables the use of build servers.</summary>
    public bool DisableBuildServers { get; set; }

    /// <summary>Forces all dependencies to be resolved and rebuilt.</summary>
    public bool Force { get; set; }

    /// <summary>Allows the command to stop and wait for user input or authentication.</summary>
    public bool Interactive { get; set; }

    /// <summary>Skips building project-to-project references.</summary>
    public bool NoDependencies { get; set; }

    /// <summary>Performs a clean build (no incremental build).</summary>
    public bool NoIncremental { get; set; }

    /// <summary>Skips the automatic restore of the project on build.</summary>
    public bool NoRestore { get; set; }

    /// <summary>Hides the startup banner and copyright message.</summary>
    public bool NoLogo { get; set; }

    /// <summary>Builds the app without bundling a self-contained runtime.</summary>
    public bool NoSelfContained { get; set; }

    /// <summary>Specifies the target operating system.</summary>
    public string? OS { get; set; }

    /// <summary>Specifies the output directory for build results.</summary>
    public string? Output { get; set; }

    /// <summary>Additional MSBuild properties to set in the format "Name=Value".</summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>Target runtime identifier (e.g., win-x64, linux-x64).</summary>
    public string? Runtime { get; set; }

    /// <summary>Indicates whether to build as self-contained (true/false).</summary>
    public bool? SelfContained { get; set; }

    /// <summary>Specifies an additional package source.</summary>
    public string? Source { get; set; }

    /// <summary>Toolset logging: auto, on, or off.</summary>
    public string? TL { get; set; }

    /// <summary>Use the current runtime instead of the one specified in the project.</summary>
    public bool? UseCurrentRuntime { get; set; }

    /// <summary>Controls the verbosity of the output (quiet, minimal, normal, detailed, diagnostic).</summary>
    public string? Verbosity { get; set; }

    /// <summary>Defines the version suffix to use.</summary>
    public string? VersionSuffix { get; set; }
    
    /// <summary>
    /// The result of the <c>dotnet build</c> command execution.
    /// </summary>
    public DotnetBuildResult? Result { get; private set; }
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the build step.");
        }

        var command = BuildCommand();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken).ConfigureAwait(false);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet build failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        Result = new DotnetBuildResult(exitCode, output, error);
        context.SetOutputFor<DotnetBuildStep, DotnetBuildResult>(Result);
    }
    
    private string BuildCommand() {
        var sb = new StringBuilder("dotnet build");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" {ProjectOrSolution}");
        if (!string.IsNullOrWhiteSpace(Architecture)) sb.Append($" --arch {Architecture}");
        if (!string.IsNullOrWhiteSpace(ArtifactsPath)) sb.Append($" --artifacts-path \"{ArtifactsPath}\"");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        if (!string.IsNullOrWhiteSpace(Framework)) sb.Append($" --framework {Framework}");
        if (DisableBuildServers) sb.Append(" --disable-build-servers");
        if (Force) sb.Append(" --force");
        if (Interactive) sb.Append(" --interactive");
        if (NoDependencies) sb.Append(" --no-dependencies");
        if (NoIncremental) sb.Append(" --no-incremental");
        if (NoRestore) sb.Append(" --no-restore");
        if (NoLogo) sb.Append(" --nologo");
        if (NoSelfContained) sb.Append(" --no-self-contained");
        if (!string.IsNullOrWhiteSpace(OS)) sb.Append($" --os {OS}");
        if (!string.IsNullOrWhiteSpace(Output)) sb.Append($" --output \"{Output}\"");
        foreach (var (key, value) in Properties)
            sb.Append($" -p:{key}={value}");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (SelfContained.HasValue) sb.Append($" --self-contained {SelfContained.Value.ToString().ToLower()}");
        if (!string.IsNullOrWhiteSpace(Source)) sb.Append($" --source \"{Source}\"");
        if (!string.IsNullOrWhiteSpace(TL)) sb.Append($" --tl:{TL}");
        if (UseCurrentRuntime.HasValue) sb.Append($" --use-current-runtime {UseCurrentRuntime.Value.ToString().ToLower()}");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");
        if (!string.IsNullOrWhiteSpace(VersionSuffix)) sb.Append($" --version-suffix {VersionSuffix}");

        return sb.ToString();
    }
}