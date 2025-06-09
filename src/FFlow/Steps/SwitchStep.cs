using FFlow.Core;

namespace FFlow;

public class SwitchStep : IFlowStep
{
    private readonly List<SwitchCase> _switches = new List<SwitchCase>();
    internal SwitchStep(List<SwitchCase> cases)
    {
        _switches = cases;
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if(_switches == null) throw new ArgumentNullException(nameof(_switches));
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var switchCase in _switches)
        {
            if (switchCase.Condition == null)
                throw new InvalidOperationException("Condition must be set for each switch case.");
            if (switchCase.Builder == null)
                throw new InvalidOperationException("Builder must be set for each switch case.");
            if (switchCase.Condition(context))
            {
                return switchCase.Builder!.Build().SetContext(context).RunAsync(context, cancellationToken);
            }
        }
        
        return Task.CompletedTask;
    }
}