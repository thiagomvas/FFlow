using FFlow.Core;

namespace FFlow;

public class SwitchCaseBuilder : ISwitchCaseBuilder
{
    private readonly List<SwitchCase> _cases = new List<SwitchCase>();

    internal IServiceProvider? _serviceProvider;
    
    public IWorkflowBuilder Case(Func<IFlowContext, bool> condition)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var builder = new FFlowBuilder(_serviceProvider);
        _cases.Add(new SwitchCase(condition, builder));
        
        return builder;
    }
    
    public IWorkflowBuilder Case<TStep>(Func<IFlowContext, bool> condition) where TStep : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        var builder = new FFlowBuilder(_serviceProvider);
        
        _cases.Add(new SwitchCase(condition, builder.StartWith<TStep>()));
        
        return builder;
    }

    public SwitchStep Build()
    {
        if (_cases.Count == 0)
        {
            throw new InvalidOperationException("At least one case must be defined.");
        }

        return new SwitchStep(_cases);
        
    }
}