using System.Diagnostics;

namespace FFlow.Steps.DotNet;

internal static class Internals
{
    public const string BaseNamespace = "FFlow.Steps.DotNet";
    public const string TargetSolutionKey = $"{BaseNamespace}.TargetSolution";
    public const string TargetProjectKey = $"{BaseNamespace}.TargetProject";
    
    public static async Task<(string Output, string Error, int ExitCode)> RunDotnetCommandAsync(
        string arguments, CancellationToken cancellationToken = default)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(outputTask, errorTask);

        process.WaitForExit();

        return (outputTask.Result, errorTask.Result, process.ExitCode);
    }
}