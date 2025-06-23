using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.Shell;

public class RunCommandStep : FlowStep
{
    public string Command { get; set; } = string.Empty;
    public string? WorkingDirectory { get; set; }
    public string? Arguments { get; set; }
    public Action<string>? OutputHandler { get; set; } = Console.WriteLine;
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Command))
            throw new InvalidOperationException("Command must be set.");
        
        if (Command.Contains(' '))
        {
            var parts = Command.Split(' ', 2);
            Command = parts[0];
            Arguments = parts.Length > 1 ? parts[1] : string.Empty + Arguments;
        }
        
        Arguments = Internals.InjectContext(Arguments ?? string.Empty, context);

        cancellationToken.ThrowIfCancellationRequested();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = $"{Arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = WorkingDirectory ?? Environment.CurrentDirectory
        };

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