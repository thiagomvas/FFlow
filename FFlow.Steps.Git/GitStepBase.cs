using FFlow.Core;

namespace FFlow.Steps.Git;

public abstract class GitStepBase : FlowStep
{
    protected readonly IGitProvider GitProvider;

    protected GitStepBase(IGitProvider provider)
    {
        GitProvider = provider ?? throw new ArgumentNullException(nameof(provider), "Git provider cannot be null.");
    }

    protected GitStepBase()
    {
        GitProvider = new GitShellProvider();
    }
    
}