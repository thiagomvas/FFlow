using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// A workflow step that executes the `dotnet pack` command to generate NuGet packages
/// from a specified .NET project or solution.
/// </summary>
public class DotnetPackStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetPackConfig();

        if (string.IsNullOrEmpty(config.ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the pack step.");
        }

        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet pack failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.SetInput(new DotnetPackResult(exitCode, output, error));
        
    }
}