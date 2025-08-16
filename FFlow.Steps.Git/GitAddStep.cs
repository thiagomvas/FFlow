using FFlow.Core;

namespace FFlow.Steps.Git;

public class GitAddStep : GitStepBase
{
    public string Path { get; set; } = string.Empty;
    public bool All { get; set; } = false;          
    public bool Force { get; set; } = false;        
    public bool Update { get; set; } = false;       

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
            throw new InvalidOperationException("Path must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        var args = new List<string>();

        if (All) args.Add("-A");
        if (Force) args.Add("-f");
        if (Update) args.Add("-u");

        args.AddRange(AdditionalArgs); 

        return GitProvider.GitAddAsync(Path, cancellationToken, args.ToArray());
    }
}
