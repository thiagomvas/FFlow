using FFlow.Core;

namespace FFlow;

public class nFlowStepBuilder : nForwardingWorkflowBuilder
{
    private readonly nFFlowBuilder _builder;
    private readonly IFlowStep _step;
    public nFlowStepBuilder(nFFlowBuilder builder, IFlowStep step) : base(builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        _step = step ?? throw new ArgumentNullException(nameof(step));
    }
    
    
}