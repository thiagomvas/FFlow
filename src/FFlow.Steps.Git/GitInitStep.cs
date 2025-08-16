using FFlow.Core;

namespace FFlow.Steps.Git;

[GitStep]
[GitStep("directoryPath", "DirectoryPath")]
public class GitInitStep : GitStepBase
{
    public string DirectoryPath { get; set; } = string.Empty;

    public bool Bare { get; set; } = false;  
    public bool Quiet { get; set; } = false; 

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        string path = string.IsNullOrWhiteSpace(DirectoryPath)
            ? Directory.GetCurrentDirectory()
            : DirectoryPath;

        cancellationToken.ThrowIfCancellationRequested();

        var args = new List<string>();
        if (Bare) args.Add("--bare");
        if (Quiet) args.Add("-q");

        args.AddRange(AdditionalArgs); 

        return GitProvider.GitInitializeAsync(path, cancellationToken, args.ToArray());
    }
}
