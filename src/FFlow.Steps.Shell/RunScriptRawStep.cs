using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.Shell;

public class RunScriptRawStep : FlowStep
{
    public string Script { get; set; } = string.Empty;
    public string? WorkingDirectory { get; set; }
    public Action<string>? OutputHandler { get; set; } = Console.WriteLine;
    public string? Arguments { get; set; }
    public Dictionary<string, string>? EnvironmentVariables { get; set; }
    public string ShellPath { get; set; } = "/bin/bash"; // Default to bash, can be overridden

    private string _tempScriptPath = string.Empty;

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Script))
            throw new InvalidOperationException("Script must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        Script = Internals.InjectContext(Script, context);

        _tempScriptPath = Path.GetTempFileName();
        await File.WriteAllTextAsync(_tempScriptPath, Script, cancellationToken);

        // Set executable bit on Unix-like systems
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            var chmod = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/chmod",
                    Arguments = $"+x \"{_tempScriptPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            chmod.Start();
            await chmod.WaitForExitAsync(cancellationToken);
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = ShellPath,
            Arguments = $"\"{_tempScriptPath}\" {Arguments ?? string.Empty}",
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

        await process.WaitForExitAsync(cancellationToken);

        var exitCode = process.ExitCode;

        try
        {
            File.Delete(_tempScriptPath);
        }
        catch
        {
            // Silent cleanup failure
        }
        context.SetOutputFor<RunScriptRawStep, int>(exitCode);
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(_tempScriptPath) && File.Exists(_tempScriptPath))
        {
            try
            {
                File.Delete(_tempScriptPath);
            }
            catch (Exception ex)
            {
                OutputHandler?.Invoke($"Failed to delete temporary script file: {ex.Message}");
            }
        }

        return Task.CompletedTask;
    }
}
