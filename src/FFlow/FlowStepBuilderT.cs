using System.Linq.Expressions;
using FFlow.Core;

namespace FFlow;

public class FlowStepBuilder<TStep> : FlowStepBuilder
    where TStep : class, IFlowStep
{
    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, TStep step) : base(workflowBuilder, step)
    {
    }
    
    public IConfigurableStepBuilder Input(
        Action<TStep> setValues)
    {
        return Input<TStep>(setValues);
    }
    
    public IConfigurableStepBuilder Input<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        TValue value)
    {
        return Input<TStep, TValue>(stepProp, value);
    }
    
    public IConfigurableStepBuilder Input<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Func<IFlowContext, TValue> inputGetter)
    {
        return Input<TStep, TValue>(stepProp, inputGetter);
    }
    
    public IConfigurableStepBuilder Output<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Action<IFlowContext, TValue> outputWriter)
    {
        return Output<TStep, TValue>(stepProp, outputWriter);
    }
    
    
}