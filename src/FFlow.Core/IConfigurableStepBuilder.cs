using System.Linq.Expressions;

namespace FFlow.Core;

public interface IConfigurableStepBuilder : IWorkflowBuilder
{
    public IWorkflowBuilder Builder { get; }
    public IConfigurableStepBuilder Input<TStep, TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Func<IFlowContext, TValue> inputGetter)
        where TStep : class, IFlowStep;
    
    public IConfigurableStepBuilder Output<TStep, TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Action<IFlowContext, TValue> outputWriter)
        where TStep : class, IFlowStep;
}