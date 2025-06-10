using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Executes the <c>dotnet restore</c> command for a specified project or solution.
/// Throws if no project or solution is specified or if the restore fails.
/// </summary>
public class DotnetRestoreStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetRestoreConfig();

        if (string.IsNullOrEmpty(config.ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the restore step.");
        }

        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet restore failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }
        
        
        context.SetInput(new DotnetRestoreResult(exitCode, output, error));
    }
    
}