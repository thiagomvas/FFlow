namespace FFlow.Steps.DotNet;

public class DotnetFlowConfiguration
{
    /// <summary>
    /// Solution file path to target.
    /// </summary>
    public string TargetSolution { get; set; } = string.Empty;

    /// <summary>
    /// Project file path (csproj) to target.
    /// </summary>
    public string TargetProject { get; set; } = string.Empty;

    /// <summary>
    /// Skip restore before build/run.
    /// </summary>
    public bool NoRestore { get; set; } = false;

    /// <summary>
    /// Skip build before run/test.
    /// </summary>
    public bool NoBuild { get; set; } = false;

    /// <summary>
    /// Build configuration (e.g., Debug or Release).
    /// </summary>
    public string Configuration { get; set; } = "Release";

    /// <summary>
    /// Target framework (e.g., net6.0, net7.0).
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Runtime identifier (e.g., win-x64, linux-x64).
    /// </summary>
    public string Runtime { get; set; } = string.Empty;

    /// <summary>
    /// Do not build project-to-project dependencies.
    /// </summary>
    public bool NoDependencies { get; set; } = false;

    /// <summary>
    /// Enable verbose logging/output.
    /// </summary>
    public bool Verbose { get; set; } = false;

    /// <summary>
    /// Custom output directory for build artifacts.
    /// </summary>
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Additional CLI arguments for dotnet commands.
    /// </summary>
    public string AdditionalArgs { get; set; } = string.Empty;
    
    public string ToRestoreArgs()
    {
        var args = new List<string>();

        if (NoDependencies)
            args.Add("--no-dependencies");

        if (Verbose)
            args.Add("-v diag");

        if (!string.IsNullOrEmpty(Runtime))
            args.Add($"--runtime {Runtime}");

        if (!string.IsNullOrWhiteSpace(AdditionalArgs))
            args.Add(AdditionalArgs);

        return string.Join(" ", args);
    }
    
    public string ToBuildArgs()
    {
        var args = new List<string>();

        if (NoRestore)
            args.Add("--no-restore");

        if (NoDependencies)
            args.Add("--no-dependencies");

        if (!string.IsNullOrEmpty(Configuration))
            args.Add($"--configuration {Configuration}");

        if (!string.IsNullOrEmpty(Framework))
            args.Add($"--framework {Framework}");

        if (!string.IsNullOrEmpty(Runtime))
            args.Add($"--runtime {Runtime}");

        if (!string.IsNullOrEmpty(OutputDirectory))
            args.Add($"--output \"{OutputDirectory}\"");

        if (Verbose)
            args.Add("-v diag");

        if (!string.IsNullOrWhiteSpace(AdditionalArgs))
            args.Add(AdditionalArgs);

        return string.Join(" ", args);
    }

    public string ToTestArgs()
    {
        var args = new List<string>();

        if (NoRestore)
            args.Add("--no-restore");

        if (NoBuild)
            args.Add("--no-build");

        if (!string.IsNullOrEmpty(Configuration))
            args.Add($"--configuration {Configuration}");

        if (!string.IsNullOrWhiteSpace(Framework))
            args.Add($"--framework {Framework}");

        if (!string.IsNullOrWhiteSpace(Runtime))
            args.Add($"--runtime {Runtime}");

        if (!string.IsNullOrWhiteSpace(OutputDirectory))
            args.Add($"--results-directory \"{OutputDirectory}\"");

        if (Verbose)
            args.Add("-v diag");

        if (!string.IsNullOrWhiteSpace(AdditionalArgs))
            args.Add(AdditionalArgs);

        return string.Join(" ", args);
    }



}

