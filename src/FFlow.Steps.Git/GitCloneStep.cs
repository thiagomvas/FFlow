using FFlow.Core;

namespace FFlow.Steps.Git;

[GitStep("repositoryUrl", "RepositoryUrl")]
public class GitCloneStep : GitStepBase
{
    
    public string RepositoryUrl { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;
    
    public string? Branch { get; set; }
    public string[]? AdditionalArgs { get; set; }
    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(RepositoryUrl))
            throw new InvalidOperationException("Repository URL must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        List<string> args = [..AdditionalArgs ?? []];
        if (!string.IsNullOrWhiteSpace(Branch))
        {
            args.Add("--branch");
            args.Add(Branch);
        }

        await GitProvider.GitCloneAsync(RepositoryUrl, LocalPath, cancellationToken, args.ToArray())
            .ConfigureAwait(false);
    }
}