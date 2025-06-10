namespace FFlow.Steps.DotNet;

using System.Collections.Generic;
using System.Text;

/// <summary>
/// Configuration options for the <c>dotnet run</c> command.
/// </summary>
public class DotnetRunConfiguration
{
    /// <summary>Target architecture (e.g., x64, x86, arm).</summary>
    public string? Architecture { get; set; }

    /// <summary>Build configuration (e.g., Debug or Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Environment variables passed as key=value pairs.</summary>
    public Dictionary<string, string> Environment { get; set; } = new();

    /// <summary>Target framework to run (e.g., net7.0).</summary>
    public string? Framework { get; set; }

    /// <summary>Forces all dependencies to be resolved and rebuilt.</summary>
    public bool Force { get; set; }

    /// <summary>Allows the command to stop and wait for user input or authentication.</summary>
    public bool Interactive { get; set; }

    /// <summary>Name of the launch profile to use.</summary>
    public string? LaunchProfile { get; set; }

    /// <summary>Skips building the project before running.</summary>
    public bool NoBuild { get; set; }

    /// <summary>Skips project-to-project dependencies when running.</summary>
    public bool NoDependencies { get; set; }

    /// <summary>Skips restoring packages before running.</summary>
    public bool NoRestore { get; set; }

    /// <summary>Target operating system.</summary>
    public string? OS { get; set; }

    /// <summary>Path to the project to run.</summary>
    public string? Project { get; set; }

    /// <summary>Target runtime identifier (e.g., win-x64, linux-x64).</summary>
    public string? Runtime { get; set; }

    /// <summary>Toolset logging level: auto, on, or off.</summary>
    public string? TL { get; set; }

    /// <summary>Controls the verbosity of the output (quiet, minimal, normal, detailed, diagnostic).</summary>
    public string? Verbosity { get; set; }

    /// <summary>Arguments to pass to the application after '--'.</summary>
    public List<string> ApplicationArguments { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder("dotnet run");

        if (!string.IsNullOrWhiteSpace(Architecture)) sb.Append($" --arch {Architecture}");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        foreach (var (key, value) in Environment)
            sb.Append($" --environment {key}={value}");
        if (!string.IsNullOrWhiteSpace(Framework)) sb.Append($" --framework {Framework}");
        if (Force) sb.Append(" --force");
        if (Interactive) sb.Append(" --interactive");
        if (!string.IsNullOrWhiteSpace(LaunchProfile)) sb.Append($" --launch-profile {LaunchProfile}");
        if (NoBuild) sb.Append(" --no-build");
        if (NoDependencies) sb.Append(" --no-dependencies");
        if (NoRestore) sb.Append(" --no-restore");
        if (!string.IsNullOrWhiteSpace(OS)) sb.Append($" --os {OS}");
        if (!string.IsNullOrWhiteSpace(Project)) sb.Append($" --project {Project}");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (!string.IsNullOrWhiteSpace(TL)) sb.Append($" --tl:{TL}");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");

        if (ApplicationArguments.Count > 0)
        {
            sb.Append(" --");
            foreach (var arg in ApplicationArguments)
            {
                if (arg.Contains(' '))
                    sb.Append($" \"{arg}\"");
                else
                    sb.Append($" {arg}");
            }
        }

        return sb.ToString();
    }
}
