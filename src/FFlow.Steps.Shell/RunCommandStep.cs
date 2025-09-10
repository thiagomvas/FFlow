using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.Shell;

[StepName("Shell Command Execution")]
[StepTags("shell")]
public class RunCommandStep : FlowStep
{
    public string Command { get; set; } = string.Empty;
    public string? WorkingDirectory { get; set; }
    public string? Arguments { get; set; }
    public Dictionary<string, string>? EnvironmentVariables { get; set; }
    public Action<string>? OutputHandler { get; set; } = Console.WriteLine;

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Command))
            throw new InvalidOperationException("Command must be set.");

        if (Command.Contains(' '))
        {
            var parts = Command.Split(' ', 2);
            Command = parts[0];
            Arguments = (parts.Length > 1 ? parts[1] + " " : "") + (Arguments ?? string.Empty);
        }

        Arguments = Internals.InjectContext(Arguments ?? string.Empty, context);

        cancellationToken.ThrowIfCancellationRequested();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = Arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = WorkingDirectory ?? Environment.CurrentDirectory
        };

        if (EnvironmentVariables != null)
        {
            foreach (var kvp in EnvironmentVariables)
            {
                processStartInfo.Environment[kvp.Key] = kvp.Value;
            }
        }

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OutputHandler?.Invoke(e.Data);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OutputHandler?.Invoke(e.Data);
        };


        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

        var exitCode = process.ExitCode;
        context.SetOutputFor<RunCommandStep, int>(exitCode);
    }
}
