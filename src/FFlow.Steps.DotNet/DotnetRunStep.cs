using FFlow.Core;

namespace FFlow.Steps.DotNet;

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