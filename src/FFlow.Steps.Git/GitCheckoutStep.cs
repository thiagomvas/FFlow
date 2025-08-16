using FFlow.Core;

namespace FFlow.Steps.Git;

[GitStep("branchName", "BranchName")]
public class GitCheckoutStep : GitStepBase
{
    public string BranchName { get; set; } = string.Empty;
    public string[]? AdditionalArgs { get; set; }

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(BranchName))
            throw new InvalidOperationException("Branch name must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        await GitProvider.GitCheckoutAsync(BranchName, cancellationToken, AdditionalArgs ?? [])
            .ConfigureAwait(false);
    }
}