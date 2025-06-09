using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

public class DotnetRestoreStep : IFlowStep
{
    /// <inheritdoc />
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var solutionPath = context.GetTargetSolution();
        var projectPath = context.GetTargetProject();

        if (string.IsNullOrEmpty(solutionPath) && string.IsNullOrEmpty(projectPath))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the restore step.");
        }

        var command = string.IsNullOrEmpty(solutionPath) 
            ? $"restore \"{projectPath}\"" 
            : $"restore \"{solutionPath}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet restore failed with exit code {process.ExitCode}.\nOutput: {output}\nError: {error}");
        }
        
        context.Set("DotnetRestoreOutput", output);
        context.Set("DotnetRestoreError", error);
        context.Set("DotnetRestoreExitCode", process.ExitCode);
    }
    
}