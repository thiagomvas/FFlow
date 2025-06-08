namespace FFlow.Core;

public abstract class ForwardingWorkflowBuilder : IWorkflowBuilder
{
    protected abstract IWorkflowBuilder Delegate { get; }
    public IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.StartWith<TStep>();
    }

    public IWorkflowBuilder StartWith(FlowAction setupAction)
    {
        return Delegate.StartWith(setupAction);
    }

    public IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Then<TStep>();
    }

    public IWorkflowBuilder Then(FlowAction setupAction)
    {
        return Delegate.Then(setupAction);
    }

    public IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Finally<TStep>();
    }

    public IWorkflowBuilder Finally(FlowAction setupAction)
    {
        return Delegate.Finally(setupAction);
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, FlowAction then, FlowAction? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, FlowAction? otherwise = null) where TTrue : class, IFlowStep
    {
        return Delegate.If<TTrue>(condition, otherwise);
    }

    public IWorkflowBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition) where TTrue : class, IFlowStep where TFalse : class, IFlowStep
    {
        return Delegate.If<TTrue, TFalse>(condition);
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IWorkflowBuilder> then, Func<IWorkflowBuilder>? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, FlowAction action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, FlowAction action) where TItem : class
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        return Delegate.ForEach<TStepIterator>(itemsSelector);
    }

    public IWorkflowBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        return Delegate.ForEach<TStepIterator, TItem>(itemsSelector);
    }

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ContinueWith<TWorkflowDefinition>() where TWorkflowDefinition : class, IWorkflowDefinition
    {
        return Delegate.ContinueWith<TWorkflowDefinition>();
    }

    public IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext
    {
        return Delegate.UseContext<TContext>();
    }

    public IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.OnAnyError<TStep>();
    }

    public IWorkflowBuilder OnAnyError(FlowAction errorHandlerAction)
    {
        return Delegate.OnAnyError(errorHandlerAction);
    }

    public IWorkflow Build()
    {
        return Delegate.Build();
    }
}