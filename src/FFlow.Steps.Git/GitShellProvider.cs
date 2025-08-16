using System.Diagnostics;

namespace FFlow.Steps.Git;

public class GitShellProvider : IGitProvider
{
    public Task GitCloneAsync(string repositoryUrl, string localPath, CancellationToken cancellation,
        params string[]? additionalArgs)
    {
        var args = $"clone {repositoryUrl}";
        if (!string.IsNullOrWhiteSpace(localPath))
        {
            args += $" \"{localPath}\"";
        }
        if (additionalArgs != null && additionalArgs.Length > 0)
        {
            args += " " + string.Join(" ", additionalArgs);
        }

        return RunGitCommandAsync(args, Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitAddAsync(string path, CancellationToken cancellation, params string[]? additionalArgs)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path must be set", nameof(path));

        // Build git add command
        var args = new List<string> { "add", path };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitCommitAsync(string commitMessage, CancellationToken cancellation, params string[]? additionalArgs)
    {
        if (string.IsNullOrWhiteSpace(commitMessage))
            throw new ArgumentException("Commit message must be set", nameof(commitMessage));
        
        var args = new List<string> { "commit", "-m", $"\"{commitMessage}\"" };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitInitializeAsync(string localPath, CancellationToken cancellation, params string[]? additionalArgs)
    {
        if (string.IsNullOrWhiteSpace(localPath))
            throw new ArgumentException("Local path must be set", nameof(localPath));

        var args = new List<string> { "init", $"\"{localPath}\"" };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitCheckoutAsync(string branchName, CancellationToken cancellation, params string[]? additionalArgs)
    {
        if (string.IsNullOrWhiteSpace(branchName))
            throw new ArgumentException("Branch name must be set", nameof(branchName));

        // Build git checkout command
        var args = new List<string> { "checkout", branchName };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitPushAsync(string remoteName, string branchName, CancellationToken cancellation,
        params string[]? additionalArgs)
    {
        // Build git push command
        var args = new List<string> { "push", remoteName, branchName };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
    }

    public Task GitPullAsync(string remoteName, string branchName, CancellationToken cancellation,
        params string[]? additionalArgs)
    {
        // Build git pull command
        var args = new List<string> { "pull", remoteName, branchName };
        if (additionalArgs != null && additionalArgs.Length > 0)
            args.AddRange(additionalArgs);

        return RunGitCommandAsync(string.Join(" ", args), Directory.GetCurrentDirectory(), cancellation);
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