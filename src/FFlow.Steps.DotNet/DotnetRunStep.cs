using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Executes the <c>dotnet run</c> command for a specified project or solution.
/// Throws if no project or solution is specified or if the run fails.
/// </summary>
public class DotnetRunStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetRunConfig();

        if (string.IsNullOrEmpty(config.Project))
        {
            throw new InvalidOperationException("A project must be specified for the run step.");
        }

        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet run failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.SetInput(new DotnetRunResult(exitCode, output, error));
    }
}