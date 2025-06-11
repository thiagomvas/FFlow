namespace FFlow.Core;

/// <summary>
/// Represents a base class for workflow builders that forwards calls to another <see cref="IWorkflowBuilder"/> instance.
/// </summary>
public abstract class ForwardingWorkflowBuilder : IWorkflowBuilder
{
    /// <summary>
    /// The underlying workflow builder that this class forwards calls to.
    /// </summary>
    protected abstract IWorkflowBuilder Delegate { get; }
    public IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.StartWith<TStep>();
    }

    public IWorkflowBuilder StartWith(AsyncFlowAction setupAction)
    {
        return Delegate.StartWith(setupAction);
    }

    public IWorkflowBuilder StartWith(SyncFlowAction setupAction)
    {
        return Delegate.StartWith(setupAction);
    }

    public IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Then<TStep>();
    }

    public IWorkflowBuilder Then(AsyncFlowAction setupAction)
    {
        return Delegate.Then(setupAction);
    }

    public IWorkflowBuilder Then(SyncFlowAction setupAction)
    {
        return Delegate.Then(setupAction);
    }

    public IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Finally<TStep>();
    }

    public IWorkflowBuilder Finally(AsyncFlowAction setupAction)
    {
        return Delegate.Finally(setupAction);
    }

    public IWorkflowBuilder Finally(SyncFlowAction setupAction)
    {
        return Delegate.Finally(setupAction);
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, AsyncFlowAction then, AsyncFlowAction? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, AsyncFlowAction? otherwise = null) where TTrue : class, IFlowStep
    {
        return Delegate.If<TTrue>(condition, otherwise);
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, SyncFlowAction then, SyncFlowAction? otherwise = null)
    {

        return Delegate.If(condition, then, otherwise);
    }

    public IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, SyncFlowAction? otherwise = null) where TTrue : class, IFlowStep
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

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, AsyncFlowAction action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, AsyncFlowAction action) where TItem : class
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, SyncFlowAction action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, SyncFlowAction action) where TItem : class
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

    public IWorkflowBuilder Switch(Action<ISwitchCaseBuilder> caseBuilder)
    {
        return Delegate.Switch(caseBuilder);
    }

    public IWorkflowBuilder Fork(ForkStrategy strategy, params Func<IWorkflowBuilder>[] forks)
    {
        return Delegate.Fork(strategy, forks);
    }

    public IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext
    {
        return Delegate.UseContext<TContext>();
    }

    public IWorkflowBuilder UseContext(IFlowContext context)
    {
        return Delegate.UseContext(context);
    }

    public IWorkflowBuilder SetProvider(IServiceProvider provider)
    {
        return Delegate.SetProvider(provider);
    }

    public IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.OnAnyError<TStep>();
    }

    public IWorkflowBuilder OnAnyError(AsyncFlowAction errorHandlerAction)
    {
        return Delegate.OnAnyError(errorHandlerAction);
    }

    public IWorkflowBuilder OnAnyError(SyncFlowAction errorHandlerAction)
    {
        return Delegate.OnAnyError(errorHandlerAction);
    }

    public IWorkflowBuilder Delay(int milliseconds)
    {
        return Delegate.Delay(milliseconds);
    }

    public IWorkflowBuilder Delay(TimeSpan delay)
    {
        return Delegate.Delay(delay);
    }

    public virtual IWorkflow Build()
    {
        return Delegate.Build();
    }
}