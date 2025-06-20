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

    public IConfigurableStepBuilder AddStep(IFlowStep step)
    {
        return Delegate.AddStep(step);
    }

    public IConfigurableStepBuilder StartWith<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.StartWith<TStep>();
    }

    public IConfigurableStepBuilder StartWith(AsyncFlowAction setupAction)
    {
        return Delegate.StartWith(setupAction);
    }

    public IConfigurableStepBuilder StartWith(SyncFlowAction setupAction)
    {
        return Delegate.StartWith(setupAction);
    }

    public IConfigurableStepBuilder Then<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Then<TStep>();
    }

    public IConfigurableStepBuilder Then(AsyncFlowAction setupAction)
    {
        return Delegate.Then(setupAction);
    }

    public IConfigurableStepBuilder Then(SyncFlowAction setupAction)
    {
        return Delegate.Then(setupAction);
    }

    public IConfigurableStepBuilder Finally<TStep>() where TStep : class, IFlowStep
    {
        return Delegate.Finally<TStep>();
    }

    public IConfigurableStepBuilder Finally(AsyncFlowAction setupAction)
    {
        return Delegate.Finally(setupAction);
    }

    public IConfigurableStepBuilder Finally(SyncFlowAction setupAction)
    {
        return Delegate.Finally(setupAction);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, AsyncFlowAction then,
        AsyncFlowAction? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IConfigurableStepBuilder If<TTrue>(Func<IFlowContext, bool> condition, AsyncFlowAction? otherwise = null)
        where TTrue : class, IFlowStep
    {
        return Delegate.If<TTrue>(condition, otherwise);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, SyncFlowAction then,
        SyncFlowAction? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IConfigurableStepBuilder If<TTrue>(Func<IFlowContext, bool> condition, SyncFlowAction? otherwise = null)
        where TTrue : class, IFlowStep
    {
        return Delegate.If<TTrue>(condition, otherwise);
    }

    public IConfigurableStepBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition)
        where TTrue : class, IFlowStep where TFalse : class, IFlowStep
    {
        return Delegate.If<TTrue, TFalse>(condition);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, Func<IConfigurableStepBuilder> then,
        Func<IConfigurableStepBuilder>? otherwise = null)
    {
        return Delegate.If(condition, then, otherwise);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, AsyncFlowAction action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, AsyncFlowAction action)
        where TItem : class
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, SyncFlowAction action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, SyncFlowAction action)
        where TItem : class
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector)
        where TStepIterator : class, IFlowStep
    {
        return Delegate.ForEach<TStepIterator>(itemsSelector);
    }

    public IConfigurableStepBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector)
        where TStepIterator : class, IFlowStep
    {
        return Delegate.ForEach<TStepIterator, TItem>(itemsSelector);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector,
        Func<IConfigurableStepBuilder> action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector,
        Func<IConfigurableStepBuilder> action)
    {
        return Delegate.ForEach(itemsSelector, action);
    }

    public IConfigurableStepBuilder ContinueWith<TWorkflowDefinition>() where TWorkflowDefinition : class, IWorkflowDefinition
    {
        return Delegate.ContinueWith<TWorkflowDefinition>();
    }

    public IConfigurableStepBuilder Switch(Action<ISwitchCaseBuilder> caseBuilder)
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

    public IWorkflowBuilder Throw(string message)
    {
        return Delegate.Throw(message);
    }

    public IWorkflowBuilder Throw<TException>(string message) where TException : Exception, new()
    {
        return Delegate.Throw<TException>(message);
    }

    public IWorkflowBuilder ThrowIf(Func<IFlowContext, bool> condition, string message)
    {
        return Delegate.ThrowIf(condition, message);
    }

    public IWorkflowBuilder ThrowIf<TException>(Func<IFlowContext, bool> condition, string message)
        where TException : Exception, new()
    {
        return Delegate.ThrowIf<TException>(condition, message);
    }

    public IWorkflowBuilder InsertStepAt(int index, IFlowStep step)
    {
        return Delegate.InsertStepAt(index, step);
    }

    public int GetStepCount()
    {
        return Delegate.GetStepCount();
    }

    public IWorkflowBuilder WithDecorator<TDecorator>(Func<IFlowStep, TDecorator> decoratorFactory) where TDecorator : BaseStepDecorator
    {
        return Delegate.WithDecorator(decoratorFactory);
    }

    public virtual IWorkflow Build()
    {
        return Delegate.Build();
    }
}