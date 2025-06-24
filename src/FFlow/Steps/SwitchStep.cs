using FFlow.Core;

namespace FFlow;

public class SwitchStep : FlowStep
{
    private readonly List<SwitchCase> _switches = new List<SwitchCase>();
    private IWorkflow _execution;
    internal SwitchStep(List<SwitchCase> cases)
    {
        _switches = cases;
    }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
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
                _execution = switchCase.Builder!.Build();
                return _execution.SetContext(context).RunAsync(context, cancellationToken);
            }
        }
        
        return Task.CompletedTask;
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_switches == null) throw new ArgumentNullException(nameof(_switches));
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var switchCase in _switches)
        {
            if (switchCase.Builder == null)
                throw new InvalidOperationException("Builder must be set for each switch case.");
            if (switchCase.Condition(context))
            {
                return _execution.CompensateAsync(cancellationToken);
            }
        }
        
        return Task.CompletedTask;
    }
}