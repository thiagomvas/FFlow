using FFlow.Core;

namespace FFlow;

public class FFlowBuilder : IWorkflowBuilder
{
    private readonly List<IFlowStep> _steps = new List<IFlowStep>();
    private IFlowStep? _errorHandler;
    private readonly IServiceProvider? _serviceProvider;
    
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

    public IWorkflowBuilder StartWith(Func<IFlowContext, Task> setupAction)
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

    public IWorkflowBuilder Then(Func<IFlowContext, Task> setupAction)
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

    public IWorkflowBuilder Finally(Func<IFlowContext, Task> setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IFlowContext, Task> then, Func<IFlowContext, Task>? otherwise = null)
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

    public IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, Func<IFlowContext, Task>? otherwise = null) where TTrue : class, IFlowStep
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

    public IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep
    {
        var step = GetOrCreateStep<TStep>();
        _errorHandler = step ?? throw new InvalidOperationException($"Could not create instance of {typeof(TStep).Name}");
        return this;
    }

    public IWorkflowBuilder OnAnyError(Func<IFlowContext, Task> errorHandlerAction)
    {
        if (errorHandlerAction == null) throw new ArgumentNullException(nameof(errorHandlerAction));
        
        _errorHandler = new DelegateFlowStep(errorHandlerAction);
        return this;
    }

    public IWorkflow Build()
    {
        var context = new InMemoryFFLowContext();
        var result = new Workflow(_steps, context);
        
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