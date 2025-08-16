namespace FFlow.Steps.Git;

public interface IGitProvider
{
    Task GitCloneAsync(string repositoryUrl, string localPath, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitCommitAsync(string commitMessage, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitAddAsync(string path, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitInitializeAsync(string localPath, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitCheckoutAsync(string branchName, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitPushAsync(string remoteName, string branchName, CancellationToken cancellation, params string[]? additionalArgs);
    Task GitPullAsync(string remoteName, string branchName, CancellationToken cancellation, params string[]? additionalArgs);
    
}