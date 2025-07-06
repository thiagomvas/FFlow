using System.Text;
using System.Text.RegularExpressions;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Executes the <c>dotnet test</c> command for a specified project or solution.
/// Throws if no project or solution is specified or if the run fails.
/// </summary>
[StepName(".NET Test")]
[StepTags("dotnet")]
[DotnetStep("projectOrSolution", "ProjectOrSolution")]
public class DotnetTestStep : IFlowStep
{
    
    /// <summary>The project, solution, directory, DLL, or EXE to test.</summary>
    public string? ProjectOrSolution { get; set; }

    /// <summary>Path to the test adapter.</summary>
    public string? TestAdapterPath { get; set; }

    /// <summary>Target architecture (e.g., x64, x86, arm).</summary>
    public string? Architecture { get; set; }

    /// <summary>Directory to place test artifacts.</summary>
    public string? ArtifactsPath { get; set; }

    /// <summary>Enable blame to collect info on test failures.</summary>
    public bool Blame { get; set; }

    /// <summary>Enable blame to collect info on crashes.</summary>
    public bool BlameCrash { get; set; }

    /// <summary>Dump type to collect on crashes (e.g., full, mini).</summary>
    public string? BlameCrashDumpType { get; set; }

    /// <summary>Always collect crash dumps even if process exits normally.</summary>
    public bool BlameCrashCollectAlways { get; set; }

    /// <summary>Enable blame to collect info on hangs.</summary>
    public bool BlameHang { get; set; }

    /// <summary>Dump type to collect on hangs (e.g., full, mini).</summary>
    public string? BlameHangDumpType { get; set; }

    /// <summary>Timeout for hang detection.</summary>
    public TimeSpan? BlameHangTimeout { get; set; }

    /// <summary>Build configuration (e.g., Debug, Release).</summary>
    public string? Configuration { get; set; }

    /// <summary>Name of data collector to use.</summary>
    public string? Collect { get; set; }

    /// <summary>Path to diagnostic log file.</summary>
    public string? DiagnosticLogFile { get; set; }

    /// <summary>Target framework to test against.</summary>
    public string? Framework { get; set; }

    /// <summary>Environment variables in the format name="value".</summary>
    public Dictionary<string, string> EnvironmentVariables { get; } = new();

    /// <summary>Filter expression to select tests to run.</summary>
    public string? Filter { get; set; }

    /// <summary>Allow interactive command prompts.</summary>
    public bool Interactive { get; set; }

    /// <summary>Test logger to use.</summary>
    public string? Logger { get; set; }

    /// <summary>Skip building before testing.</summary>
    public bool NoBuild { get; set; }

    /// <summary>Suppress logo output.</summary>
    public bool NoLogo { get; set; }

    /// <summary>Skip restoring before testing.</summary>
    public bool NoRestore { get; set; }

    /// <summary>Directory where test results are written.</summary>
    public string? ResultsDirectory { get; set; }

    /// <summary>Output directory for build outputs.</summary>
    public string? Output { get; set; }

    /// <summary>Target OS for the test run.</summary>
    public string? OS { get; set; }

    /// <summary>Runtime identifier for the test run.</summary>
    public string? Runtime { get; set; }

    /// <summary>Path to runsettings file.</summary>
    public string? SettingsFile { get; set; }

    /// <summary>List tests without running them.</summary>
    public bool ListTests { get; set; }

    /// <summary>Verbosity level (quiet, minimal, normal, detailed, diagnostic).</summary>
    public string? Verbosity { get; set; }

    /// <summary>Additional args passed after '--'.</summary>
    public List<string> AdditionalArgs { get; } = new();

    /// <summary>Additional RunSettings arguments passed after '--'.</summary>
    public List<string> RunSettingsArguments { get; } = new();
    
    /// <summary>
    /// The result of the <c>dotnet test</c> command.
    /// </summary>
    public DotnetTestResult? Result { get; private set; }
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ProjectOrSolution))
            throw new InvalidOperationException("Either a solution or a project must be specified for the test step.");

        var command = BuildCommand();
        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
            throw new InvalidOperationException($"Dotnet test failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");

        // Parse test summary from output (example pattern parsing)
        var passed = ParseTestCount(output, "Passed");
        var failed = ParseTestCount(output, "Failed");
        var skipped = ParseTestCount(output, "Skipped");

        Result = new DotnetTestResult
        {
            Output = output,
            Error = error,
            ExitCode = exitCode,
            Passed = passed,
            Failed = failed,
            Skipped = skipped
        };

        context.SetOutputFor<DotnetTestStep, DotnetTestResult>(Result);
    }

    private int ParseTestCount(string output, string key)
    {
        var pattern = $@"{key}:\s*(\d+)";
        var matches = Regex.Matches(output, pattern);
        int total = 0;

        foreach (Match match in matches)
        {
            if (int.TryParse(match.Groups[1].Value, out int count))
                total += count;
        }

        return total;
    }


    private string BuildCommand()
    {
        var sb = new StringBuilder("dotnet test");

        if (!string.IsNullOrWhiteSpace(ProjectOrSolution)) sb.Append($" \"{ProjectOrSolution}\"");
        if (!string.IsNullOrWhiteSpace(TestAdapterPath)) sb.Append($" --test-adapter-path \"{TestAdapterPath}\"");
        if (!string.IsNullOrWhiteSpace(Architecture)) sb.Append($" --arch {Architecture}");
        if (!string.IsNullOrWhiteSpace(ArtifactsPath)) sb.Append($" --artifacts-path \"{ArtifactsPath}\"");
        if (Blame) sb.Append(" --blame");
        if (BlameCrash) sb.Append(" --blame-crash");
        if (!string.IsNullOrWhiteSpace(BlameCrashDumpType)) sb.Append($" --blame-crash-dump-type {BlameCrashDumpType}");
        if (BlameCrashCollectAlways) sb.Append(" --blame-crash-collect-always");
        if (BlameHang) sb.Append(" --blame-hang");
        if (!string.IsNullOrWhiteSpace(BlameHangDumpType)) sb.Append($" --blame-hang-dump-type {BlameHangDumpType}");
        if (BlameHangTimeout.HasValue) sb.Append($" --blame-hang-timeout {BlameHangTimeout.Value:c}");
        if (!string.IsNullOrWhiteSpace(Configuration)) sb.Append($" --configuration {Configuration}");
        if (!string.IsNullOrWhiteSpace(Collect)) sb.Append($" --collect {Collect}");
        if (!string.IsNullOrWhiteSpace(DiagnosticLogFile)) sb.Append($" --diag \"{DiagnosticLogFile}\"");
        if (!string.IsNullOrWhiteSpace(Framework)) sb.Append($" --framework {Framework}");
        foreach (var env in EnvironmentVariables)
            sb.Append($" --environment \"{env.Key}={env.Value}\"");
        if (!string.IsNullOrWhiteSpace(Filter)) sb.Append($" --filter \"{Filter}\"");
        if (Interactive) sb.Append(" --interactive");
        if (!string.IsNullOrWhiteSpace(Logger)) sb.Append($" --logger {Logger}");
        if (NoBuild) sb.Append(" --no-build");
        if (NoLogo) sb.Append(" --nologo");
        if (NoRestore) sb.Append(" --no-restore");
        if (!string.IsNullOrWhiteSpace(Output)) sb.Append($" --output \"{Output}\"");
        if (!string.IsNullOrWhiteSpace(OS)) sb.Append($" --os {OS}");
        if (!string.IsNullOrWhiteSpace(ResultsDirectory)) sb.Append($" --results-directory \"{ResultsDirectory}\"");
        if (!string.IsNullOrWhiteSpace(Runtime)) sb.Append($" --runtime {Runtime}");
        if (!string.IsNullOrWhiteSpace(SettingsFile)) sb.Append($" --settings \"{SettingsFile}\"");
        if (ListTests) sb.Append(" --list-tests");
        if (!string.IsNullOrWhiteSpace(Verbosity)) sb.Append($" --verbosity {Verbosity}");

        if (AdditionalArgs.Count > 0)
            sb.Append(' ').Append(string.Join(' ', AdditionalArgs));

        if (RunSettingsArguments.Count > 0)
            sb.Append(" -- ").Append(string.Join(' ', RunSettingsArguments));

        return sb.ToString();
    }
}
