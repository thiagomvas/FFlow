using FFlow.Core;

namespace FFlow.Steps.Git;

public class GitCloneStep : FlowStep
{
    private readonly IGitProvider _provider;
    
    public string RepositoryUrl { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;
    
    public string? Branch { get; set; }
    public string[]? AdditionalArgs { get; set; }

    public GitCloneStep(IGitProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider), "Git provider cannot be null.");
    }

    public GitCloneStep()
    {
        _provider = new GitShellProvider();
    }
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(RepositoryUrl))
            throw new InvalidOperationException("Repository URL must be set.");

        if (string.IsNullOrWhiteSpace(LocalPath))
            throw new InvalidOperationException("Local path must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        List<string> args = [..AdditionalArgs ?? []];
        if (!string.IsNullOrWhiteSpace(Branch))
        {
            args.Add("--branch");
            args.Add(Branch);
        }

        await _provider.GitCloneAsync(RepositoryUrl, LocalPath, cancellationToken, args.ToArray())
            .ConfigureAwait(false);
    }
}