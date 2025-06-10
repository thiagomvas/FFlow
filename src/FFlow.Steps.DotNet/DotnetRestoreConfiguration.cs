using System.Text;

namespace FFlow.Steps.DotNet;
/// <summary>
/// Configuration options for the <c>dotnet restore</c> command.
/// </summary>
public class DotnetRestoreConfiguration
{
    /// <summary>The project or solution to restore dependencies for.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Path to the NuGet configuration file to use.</summary>
    public string? ConfigFile { get; set; }

    /// <summary>Disables the use of build servers.</summary>
    public bool DisableBuildServers { get; set; }

    /// <summary>Disables restoring multiple projects in parallel.</summary>
    public bool DisableParallel { get; set; }

    /// <summary>Forces all dependencies to be resolved even if the last restore was successful.</summary>
    public bool Force { get; set; }

    /// <summary>Forces reevaluation of all dependencies.</summary>
    public bool ForceEvaluate { get; set; }

    /// <summary>Ignores any failed NuGet sources.</summary>
    public bool IgnoreFailedSources { get; set; }

    /// <summary>Allows the command to prompt for user input (e.g., for authentication).</summary>
    public bool Interactive { get; set; }

    /// <summary>Specifies the path to the lock file.</summary>
    public string? LockFilePath { get; set; }

    /// <summary>Only allows restore if the lock file matches the project graph.</summary>
    public bool LockedMode { get; set; }

    /// <summary>Disables using the NuGet package cache.</summary>
    public bool NoCache { get; set; }

    /// <summary>Skips restoring project-to-project references.</summary>
    public bool NoDependencies { get; set; }

    /// <summary>Directory where packages will be restored.</summary>
    public string? PackagesDirectory { get; set; }

    /// <summary>Target runtime identifier (e.g., win-x64, linux-x64).</summary>
    public string? Runtime { get; set; }

    /// <summary>List of NuGet package sources to use.</summary>
    public List<string> Sources { get; set; } = new();

    /// <summary>Toolset logging: auto, on, or off.</summary>
    public string? TL { get; set; }

    /// <summary>Use the current runtime instead of the one specified in the project.</summary>
    public bool? UseCurrentRuntime { get; set; }

    /// <summary>Enables the generation of a lock file.</summary>
    public bool UseLockFile { get; set; }

    /// <summary>Target architecture (e.g., x64, x86, arm).</summary>
    public string? Architecture { get; set; }

    /// <summary>Target operating system.</summary>
    public string? OS { get; set; }

    /// <summary>Verbosity level (quiet, minimal, normal, detailed, diagnostic).</summary>
    public string? Verbosity { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder("dotnet restore");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" {ProjectOrSolution}");
        if (!string.IsNullOrWhiteSpace(ConfigFile)) sb.Append($" --configfile \"{ConfigFile}\"");
        if (DisableBuildServers) sb.Append(" --disable-build-servers");
        if (DisableParallel) sb.Append(" --disable-parallel");
        if (Force) sb.Append(" --force");
        if (ForceEvaluate) sb.Append(" --force-evaluate");
        if (IgnoreFailedSources) sb.Append(" --ignore-failed-sources");
        if (Interactive) sb.Append(" --interactive");
        if (!string.IsNullOrWhiteSpace(LockFilePath)) sb.Append($" --lock-file-path \"{LockFilePath}\"");
        if (LockedMode) sb.Append(" --locked-mode");
        if (NoCache) sb.Append(" --no-cache");
        if (NoDependencies) sb.Append(" --no-dependencies");
        if (!string.IsNullOrWhiteSpace(PackagesDirectory)) sb.Append($" --packages \"{PackagesDirectory}\"");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        foreach (var source in Sources)
            sb.Append($" --source \"{source}\"");
        if (!string.IsNullOrWhiteSpace(TL)) sb.Append($" --tl:{TL}");
        if (UseCurrentRuntime.HasValue) sb.Append($" --use-current-runtime {UseCurrentRuntime.Value.ToString().ToLower()}");
        if (UseLockFile) sb.Append(" --use-lock-file");
        if (!string.IsNullOrWhiteSpace(Architecture)) sb.Append($" --arch {Architecture}");
        if (!string.IsNullOrWhiteSpace(OS)) sb.Append($" --os {OS}");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");

        return sb.ToString();
    }
}