using FFlow.Core;

namespace FFlow.Steps.DotNet;

public class DotnetBuildStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var config = context.Get<DotnetFlowConfiguration>(Internals.BaseNamespace + ".Configuration");

        if (string.IsNullOrEmpty(config.TargetSolution) && string.IsNullOrEmpty(config.TargetProject))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the build step.");
        }

        var target = !string.IsNullOrEmpty(config.TargetSolution)
            ? $"\"{config.TargetSolution}\""
            : $"\"{config.TargetProject}\"";

        var command = $"build {target} {config.ToBuildArgs()}".Trim();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet build failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.Set("DotnetBuildOutput", output);
        context.Set("DotnetBuildError", error);
        context.Set("DotnetBuildExitCode", exitCode);
    }
}