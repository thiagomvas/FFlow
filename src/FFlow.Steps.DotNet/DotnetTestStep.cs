using System.Text.RegularExpressions;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

public class DotnetTestStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetTestConfig();
        if (string.IsNullOrEmpty(config.ProjectOrSolution))
            throw new InvalidOperationException("Either a solution or a project must be specified for the test step.");

        var command = config.ToString();
        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
            throw new InvalidOperationException($"Dotnet test failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");

        // Parse test summary from output (example pattern parsing)
        var passed = ParseTestCount(output, "Passed");
        var failed = ParseTestCount(output, "Failed");
        var skipped = ParseTestCount(output, "Skipped");

        var result = new DotnetTestResult
        {
            Output = output,
            Error = error,
            ExitCode = exitCode,
            Passed = passed,
            Failed = failed,
            Skipped = skipped
        };

        context.SetInput(result);
    }

    private int ParseTestCount(string output, string key)
    {
        // Example summary line format:
        // Passed!  - Failed:     0, Passed:    13, Skipped:     0, Total:    13, Duration: 287 ms - FFlow.Tests.dll (net9.0)
    
        // Pattern to extract e.g. "Failed: 0"
        var pattern = $@"{key}:\s*(\d+)";
        var match = Regex.Match(output, pattern);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int count))
            return count;

        return 0;
    }
}
