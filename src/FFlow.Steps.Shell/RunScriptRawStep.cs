using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.Shell;

public class RunScriptRawStep : FlowStep
{
    public string Script { get; set; } = string.Empty;
    public string? WorkingDirectory { get; set; }
    public Action<string>? OutputHandler { get; set; } = Console.WriteLine;
    public string? Arguments { get; set; }
    public string? EnvironmentVariables { get; set; }
    public string ShellPath { get; set; } = "/bin/bash"; // Default to bash, can be overridden
    
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Script))
            throw new InvalidOperationException("Script must be set.");

        cancellationToken.ThrowIfCancellationRequested();
        
        Script = Internals.InjectContext(Script, context);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = ShellPath,
            Arguments = $"-c \"{Script}\" {Arguments ?? string.Empty}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = WorkingDirectory ?? Environment.CurrentDirectory
        };

        if (!string.IsNullOrEmpty(EnvironmentVariables))
        {
            foreach (var env in EnvironmentVariables.Split(';'))
            {
                var parts = env.Split('=');
                if (parts.Length == 2)
                {
                    processStartInfo.Environment[parts[0]] = parts[1];
                }
            }
        }

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.OutputDataReceived += (sender, e) => OutputHandler?.Invoke(e.Data ?? string.Empty);
        process.ErrorDataReceived += (sender, e) => OutputHandler?.Invoke(e.Data ?? string.Empty);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync(cancellationToken);
        
        var exitCode = process.ExitCode;
        OutputHandler?.Invoke($"Process exited with code {exitCode}");
    }
}