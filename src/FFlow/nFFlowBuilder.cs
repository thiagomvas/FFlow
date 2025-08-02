using System.Linq.Expressions;
using FFlow.Core;

namespace FFlow;

public class nFFlowBuilder : WorkflowBuilderBase
{
    private WorkflowOptions? _options;
    private IFlowStep? _errorHandler;
    private IFlowStep? _finalizer;
    private IFlowStep? _starter;
    private readonly IServiceProvider? _serviceProvider;
    public nFFlowBuilder(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual nFFlowBuilder WithStarter(IFlowStep starter)
    {
        _starter = starter ?? throw new ArgumentNullException(nameof(starter));
        return this;
    }
    public virtual nFFlowBuilder WithErrorHandler(IFlowStep errorHandler)
    {
        _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        return this;
    }
    public virtual nFFlowBuilder WithFinalizer(IFlowStep finalizer)
    {
        _finalizer = finalizer ?? throw new ArgumentNullException(nameof(finalizer));
        return this;
    }
    public virtual nFFlowBuilder WithOptions(WorkflowOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        return this;
    }
    
    public virtual nFFlowBuilder WithOptions(Action<WorkflowOptions> configure)
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
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
}