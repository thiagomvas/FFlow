using System.Linq.Expressions;
using FFlow.Core;

namespace FFlow;

public class FFlowBuilder : WorkflowBuilderBase
{
    internal WorkflowOptions? _options = new();
    private IFlowStep? _finalizer;
    private IFlowStep? _starter;
    private IFlowStep? _errorHandler;
    private readonly IServiceProvider? _serviceProvider;
    private IRetryPolicy? _retryPolicy;
    public IServiceProvider Services => _serviceProvider;
    public FFlowBuilder(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }
    
    public FFlowBuilder(IServiceProvider? serviceProvider, IStepTemplateRegistry stepTemplateRegistry)
    {
        _serviceProvider = serviceProvider;
        StepTemplateRegistry = stepTemplateRegistry ?? throw new ArgumentNullException(nameof(stepTemplateRegistry));
    }

    public virtual FFlowBuilder WithStarter(IFlowStep starter)
    {
        _starter = starter ?? throw new ArgumentNullException(nameof(starter));
        return this;
    }
    public virtual FFlowBuilder WithErrorHandler(IFlowStep errorHandler)
    {
        _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        return this;
    }
    public virtual FFlowBuilder WithFinalizer(IFlowStep finalizer)
    {
        _finalizer = finalizer ?? throw new ArgumentNullException(nameof(finalizer));
        return this;
    }
    public virtual FFlowBuilder WithOptions(WorkflowOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        return this;
    }
    
    public virtual FFlowBuilder WithOptions(Action<WorkflowOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _options ??= new WorkflowOptions();
        configure(_options);
        return this;
    }
    
    internal bool TryResolveStep<TStep>(out TStep? step) where TStep : class, IFlowStep
    {
        step = default;

        try
        {
            step = _serviceProvider?.GetService(typeof(TStep)) as TStep;

            if (step == null)
            {
                // Try parameterless constructor as fallback
                var ctor = typeof(TStep).GetConstructor(Type.EmptyTypes);
                if (ctor == null) return false;

                step = Activator.CreateInstance<TStep>()!;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool TryAddStepResolved<TStep>(out TStep? step) where TStep : class, IFlowStep
    {
        if (!TryResolveStep(out step)) return false;
        AddStep(step!);
        return true;
    }


    public override IWorkflow Build()
    {
        
        var context = FlowContext
                      ?? _serviceProvider?.GetService(ContextType ?? typeof(InMemoryFFLowContext)) as IFlowContext
                      ?? Activator.CreateInstance(ContextType ?? typeof(InMemoryFFLowContext)) as IFlowContext;

        if (_starter is not null)
        {
            InsertStepAt(0, _starter);
        }
        
        if (Steps.Count == 0)
        {
            throw new InvalidOperationException("Cannot build a workflow with no steps.");
        }
        if (context == null)
        {
            throw new InvalidOperationException("Cannot build a workflow without a valid context.");
        }
        
        var result = new Workflow(Steps, context!, _options);
        
        if (_errorHandler != null)
        {
            result.SetGlobalErrorHandler(_errorHandler);
        }

        if (_finalizer is not null) result.SetFinalizer(_finalizer);
        
        return result;
    }

    public override void SetErrorHandlingStep(IFlowStep step)
    {
        _errorHandler = step ?? throw new ArgumentNullException(nameof(step));
    }
}