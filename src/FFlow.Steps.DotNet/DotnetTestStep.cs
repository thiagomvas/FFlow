using FFlow.Core;

namespace FFlow.Steps.DotNet;

public class DotnetTestStep : IFlowStep
{
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
      
        var config = context.GetDotnetTestConfig();
        if (string.IsNullOrEmpty(config.ProjectOrSolution))
        {
            throw new InvalidOperationException("Either a solution or a project must be specified for the test step.");
        }
        
        var command = config.ToString();

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Dotnet test failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");
        }

        context.Set("DotnetTestOutput", output);
        context.Set("DotnetTestError", error);
        context.Set("DotnetTestExitCode", exitCode);
    }
}