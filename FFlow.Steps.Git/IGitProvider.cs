namespace FFlow.Steps.Git;

public interface IGitProvider
{
    Task GitCloneAsync(string repositoryUrl, string localPath, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitCommitAsync(string localPath, string commitMessage, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitAddAsync(string path, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitInitializeAsync(string localPath, CancellationToken cancellation, params string[]? additionalArgs);
}