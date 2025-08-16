using System.Diagnostics;

namespace FFlow.Steps.Git;

public class GitShellProvider : IGitProvider
{
    public Task GitCloneAsync(string repositoryUrl, string localPath, CancellationToken cancellation,
        params string[]? additionalArgs)
    {
        var args = $"clone {repositoryUrl} \"{localPath}\"";
        if (additionalArgs != null && additionalArgs.Length > 0)
        {
            args += " " + string.Join(" ", additionalArgs);
        }

        return RunGitCommandAsync(args, Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitCommitAsync(string localPath, string commitMessage, CancellationToken cancellation,
        params string[]? additionalArgs)
    {
        throw new NotImplementedException();
    }

    public Task GitAddAsync(string path, CancellationToken cancellation, params string[]? additionalArgs)
    {
        throw new NotImplementedException();
    }

    public Task GitInitializeAsync(string localPath, CancellationToken cancellation, params string[]? additionalArgs)
    {
        throw new NotImplementedException();
    }
    
    private static async Task<(string Output, string Error, int ExitCode)> RunGitCommandAsync(string arguments, string workingDirectory, CancellationToken cancellationToken)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = processStartInfo };
        var outputBuilder = new System.Text.StringBuilder();
        var errorBuilder = new System.Text.StringBuilder();

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
                outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
                errorBuilder.AppendLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync(cancellationToken);

        return (outputBuilder.ToString(), errorBuilder.ToString(), process.ExitCode);
    }
}