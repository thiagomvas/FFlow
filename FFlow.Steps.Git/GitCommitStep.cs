using FFlow.Core;

namespace FFlow.Steps.Git;

public class GitCommitStep : GitStepBase
{
    public string CommitMessage { get; set; } = string.Empty;

    public bool Amend { get; set; } = false;
    public bool NoVerify { get; set; } = false;
    public bool Signoff { get; set; } = false;
    public bool All { get; set; } = false;
    public bool Quiet { get; set; } = false;

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CommitMessage))
            throw new InvalidOperationException("Commit message must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        // Build argument list dynamically
        var args = new List<string>();

        if (Amend) args.Add("--amend");
        if (NoVerify) args.Add("--no-verify");
        if (Signoff) args.Add("-s");
        if (All) args.Add("-a");
        if (Quiet) args.Add("-q");

        args.AddRange(AdditionalArgs);

        await GitProvider.GitCommitAsync(CommitMessage, cancellationToken, args.ToArray())
            .ConfigureAwait(false);
    }
}
