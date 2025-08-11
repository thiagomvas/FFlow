using System.Text;
using FFlow.Core;

namespace FFlow.Steps.DotNet;
/// <summary>
/// Executes the <c>dotnet publish</c> command for a specified project or solution.
/// Throws if no project or solution is specified or if the publish fails.
/// </summary>
[StepName(".NET Publish")]
[StepTags("dotnet")]
[DotnetStep("projectOrSolution", "ProjectOrSolution")]
public class DotnetPublishStep : IFlowStep
{
    /// <summary>The project or solution file to publish.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Specifies the target architecture (e.g., x64, x86, arm).</summary>
    public string? Architecture { get; set; }

    /// <summary>Path to the directory where artifacts will be stored.</summary>
    public string? ArtifactsPath { get; set; }

    /// <summary>Publish configuration (e.g., Debug or Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Disables the use of build servers.</summary>
    public bool DisableBuildServers { get; set; }

    /// <summary>Target framework to publish for (e.g., net7.0).</summary>
    public string? Framework { get; set; }

    /// <summary>Forces all dependencies to be resolved and rebuilt.</summary>
    public bool Force { get; set; }

    /// <summary>Allows the command to stop and wait for user input or authentication.</summary>
    public bool Interactive { get; set; }

    /// <summary>Path to a manifest file describing the publish output.</summary>
    public string? Manifest { get; set; }

    /// <summary>Skips building before publishing.</summary>
    public bool NoBuild { get; set; }

    /// <summary>Skips project-to-project dependencies when publishing.</summary>
    public bool NoDependencies { get; set; }

    /// <summary>Skips restoring packages before publishing.</summary>
    public bool NoRestore { get; set; }

    /// <summary>Hides the startup banner and copyright message.</summary>
    public bool NoLogo { get; set; }

    /// <summary>Specifies the output directory for published files.</summary>
    public string? Output { get; set; }

    /// <summary>Target operating system for the published output.</summary>
    public string? OS { get; set; }

    /// <summary>Target runtime identifier (e.g., win-x64, linux-x64).</summary>
    public string? Runtime { get; set; }

    /// <summary>Whether the published app is self-contained or framework-dependent.</summary>
    public bool? SelfContained { get; set; }

    /// <summary>Explicitly disables self-contained publishing.</summary>
    public bool NoSelfContained { get; set; }

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
    /// The result of the <c>dotnet publish</c> command.
    /// </summary>
    public DotnetPublishResult? Result { get; private set; }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the publish step.");
        }

        var command = BuildCommand();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken).ConfigureAwait(false);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet publish failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        Result = new DotnetPublishResult(exitCode, output, error);
        context.SetOutputFor<DotnetPublishStep, DotnetPublishResult>(Result);
    }

    private string BuildCommand()
    {
        var sb = new StringBuilder("dotnet publish");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" {ProjectOrSolution}");
        if (!string.IsNullOrWhiteSpace(Architecture)) sb.Append($" --arch {Architecture}");
        if (!string.IsNullOrWhiteSpace(ArtifactsPath)) sb.Append($" --artifacts-path \"{ArtifactsPath}\"");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        if (DisableBuildServers) sb.Append(" --disable-build-servers");
        if (!string.IsNullOrWhiteSpace(Framework)) sb.Append($" --framework {Framework}");
        if (Force) sb.Append(" --force");
        if (Interactive) sb.Append(" --interactive");
        if (!string.IsNullOrWhiteSpace(Manifest)) sb.Append($" --manifest \"{Manifest}\"");
        if (NoBuild) sb.Append(" --no-build");
        if (NoDependencies) sb.Append(" --no-dependencies");
        if (NoRestore) sb.Append(" --no-restore");
        if (NoLogo) sb.Append(" --nologo");
        if (!string.IsNullOrWhiteSpace(Output)) sb.Append($" --output \"{Output}\"");
        if (!string.IsNullOrWhiteSpace(OS)) sb.Append($" --os {OS}");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (SelfContained.HasValue) sb.Append($" --self-contained {SelfContained.Value.ToString().ToLower()}");
        if (NoSelfContained) sb.Append(" --no-self-contained");
        if (!string.IsNullOrWhiteSpace(Source)) sb.Append($" --source \"{Source}\"");
        if (!string.IsNullOrWhiteSpace(TL)) sb.Append($" --tl:{TL}");
        if (UseCurrentRuntime.HasValue) sb.Append($" --use-current-runtime {UseCurrentRuntime.Value.ToString().ToLower()}");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");
        if (!string.IsNullOrWhiteSpace(VersionSuffix)) sb.Append($" --version-suffix {VersionSuffix}");

        return sb.ToString();
    }
}