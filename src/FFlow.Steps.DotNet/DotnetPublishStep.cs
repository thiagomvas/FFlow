using FFlow.Core;

namespace FFlow.Steps.DotNet;

public class DotnetPublishStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.GetDotnetPublishConfig();

        if (string.IsNullOrEmpty(config.ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the publish step.");
        }

        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet publish failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.SetInput(new DotnetPublishResult(exitCode, output, error));
    }
}