using FFlow.Core;

namespace FFlow.Steps.Git;

[GitStep]
[GitStep("remoteName", "RemoteName")]
[GitStep("remoteName", "RemoteName", "branchName", "BranchName")]
public class GitPullStep : GitStepBase
{
    public string RemoteName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string[]? AdditionalArgs { get; set; }

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await GitProvider.GitPullAsync(RemoteName, BranchName, cancellationToken, AdditionalArgs ?? [])
            .ConfigureAwait(false);
    }
    
}