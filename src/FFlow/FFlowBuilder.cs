using FFlow.Core;

namespace FFlow;

public class FFlowBuilder : IWorkflowBuilder
{
    private readonly List<IFlowStep> _steps = new List<IFlowStep>();
    private IFlowStep? _errorHandler;
    private readonly IServiceProvider? _serviceProvider;
    private Type? _contextType = typeof(InMemoryFFLowContext);
    
    public FFlowBuilder(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep
    {
        var step = GetOrCreateStep<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder StartWith(FlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep
    {
        var step = GetOrCreateStep<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Then(FlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep
    {
        var step = GetOrCreateStep<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Finally(FlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, FlowAction then, FlowAction? otherwise = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (then == null) throw new ArgumentNullException(nameof(then));
        
        var trueStep = new DelegateFlowStep(then);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep(otherwise);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return this;
    }

    public IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, FlowAction? otherwise = null) where TTrue : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var trueStep = GetOrCreateStep<TTrue>();
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep(otherwise);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return this;
    }

    public IWorkflowBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition) where TTrue : class, IFlowStep where TFalse : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var trueStep = GetOrCreateStep<TTrue>();
        var falseStep = GetOrCreateStep<TFalse>();
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return this;
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IWorkflowBuilder> then, Func<IWorkflowBuilder>? otherwise = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (then == null) throw new ArgumentNullException(nameof(then));
        
        var trueBuilder = then();
        var trueStep = new BuilderStep(trueBuilder);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            var falseBuilder = otherwise();
            falseStep = new BuilderStep(falseBuilder);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return this;
    }

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, FlowAction action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var step = new ForEachStep(itemsSelector, new DelegateFlowStep(action));
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, FlowAction action) where TItem : class
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var step = new ForEachStep<TItem>(itemsSelector, new DelegateFlowStep(action));
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        
        var step = new ForEachStep(itemsSelector, GetOrCreateStep<TStepIterator>());
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        
        var step = new ForEachStep<TItem>(itemsSelector, GetOrCreateStep<TStepIterator>());
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var builder = action();
        var step = new ForEachStep(itemsSelector, new BuilderStep(builder));
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var builder = action();
        var step = new ForEachStep<TItem>(itemsSelector, new BuilderStep(builder));
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ContinueWith<TWorkflowDefinition>() where TWorkflowDefinition : class, IWorkflowDefinition
    {
        var workflowDefinition = _serviceProvider?.GetService(typeof(TWorkflowDefinition)) as TWorkflowDefinition
                                 ?? Activator.CreateInstance<TWorkflowDefinition>();
        if (workflowDefinition == null)
        {
            throw new InvalidOperationException($"Could not create instance of {typeof(TWorkflowDefinition).Name}");
        }
        
        var step = new WorkflowContinuationStep(workflowDefinition);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext
    {
        _contextType = typeof(TContext);
        return this;
    }



    public IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep
    {
        var step = GetOrCreateStep<TStep>();
        _errorHandler = step ?? throw new InvalidOperationException($"Could not create instance of {typeof(TStep).Name}");
        return this;
    }

    public IWorkflowBuilder OnAnyError(FlowAction errorHandlerAction)
    {
        if (errorHandlerAction == null) throw new ArgumentNullException(nameof(errorHandlerAction));
        
        _errorHandler = new DelegateFlowStep(errorHandlerAction);
        return this;
    }

    public IWorkflow Build()
    {
        var context = _serviceProvider?.GetService(_contextType ?? typeof(InMemoryFFLowContext)) as IFlowContext
                      ?? Activator.CreateInstance(_contextType ?? typeof(InMemoryFFLowContext)) as IFlowContext;
        var result = new Workflow(_steps, context!);
        
        if (_errorHandler != null)
        {
            result.SetGlobalErrorHandler(_errorHandler);
        }
        
        return result;
    }
    
    
    private TStep GetOrCreateStep<TStep>() where TStep : class, IFlowStep
    {
        var step = _serviceProvider?.GetService(typeof(TStep)) as TStep
                   ?? Activator.CreateInstance<TStep>();
        return step;
    }
}