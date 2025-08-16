using FFlow.Core;

namespace FFlow.Steps.Git;

public abstract class GitStepBase : FlowStep
{
    protected readonly IGitProvider GitProvider;

    private string[]? _additionalArgs;
    public string[] AdditionalArgs
    {
        get => _additionalArgs ?? [];
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), "DefaultArgs cannot be null.");
            _additionalArgs = value;
        }
    }
    protected GitStepBase(IGitProvider provider)
    {
        GitProvider = provider ?? throw new ArgumentNullException(nameof(provider), "Git provider cannot be null.");
    }

    protected GitStepBase()
    {
        GitProvider = new GitShellProvider();
    }
}