using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// A workflow step that executes the <c>dotnet build</c> command to build a .NET project or solution.
/// </summary>
public class DotnetBuildStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetBuildConfig();

        if (string.IsNullOrEmpty(config.ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the build step.");
        }


        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet build failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.SetInput(new DotnetBuildResult(exitCode, output, error));
    }
}