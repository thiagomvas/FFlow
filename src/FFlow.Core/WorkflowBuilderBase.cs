namespace FFlow.Core;

public abstract class WorkflowBuilderBase
{
    private readonly List<IFlowStep> _steps = new();
    private IServiceProvider? _serviceProvider;
    private IStepTemplateRegistry? _stepTemplateRegistry;
    private IFlowContext? _flowContext;
    private Type? _contextType;

    public virtual IReadOnlyList<IFlowStep> Steps => _steps.AsReadOnly();

    public abstract IWorkflow Build();
    
    protected IServiceProvider? ServiceProvider
    {
        get => _serviceProvider;
        set
        {
            if (_serviceProvider is not null && value is not null && _serviceProvider != value)
                throw new InvalidOperationException("ServiceProvider has already been set and cannot be changed.");
            _serviceProvider = value;
        }
    }
    
    protected IStepTemplateRegistry? StepTemplateRegistry
    {
        get => _stepTemplateRegistry;
        set
        {
            if (_stepTemplateRegistry is not null && value is not null && _stepTemplateRegistry != value)
                throw new InvalidOperationException("StepTemplateRegistry has already been set and cannot be changed.");
            _stepTemplateRegistry = value;
        }
    }
    
    protected IFlowContext? FlowContext
    {
        get => _flowContext;
        set
        {
            if (_flowContext is not null && value is not null && _flowContext != value)
                throw new InvalidOperationException("FlowContext has already been set and cannot be changed.");
            _flowContext = value;
        }
    }
    
    protected Type? ContextType
    {
        get => _contextType;
        set
        {
            if (_contextType is not null && value is not null && _contextType != value)
                throw new InvalidOperationException("ContextType has already been set and cannot be changed.");
            _contextType = value;
        }
    }

    public virtual IConfigurableStepBuilder AddStep<TStep>(TStep step)
        where TStep : IFlowStep
    {
        if (step is null)
            throw new ArgumentNullException(nameof(step));

        _steps.Add(step);
        return CreateConfigurableBuilder(step);
    }

    public virtual IConfigurableStepBuilder InsertStepAt<TStep>(int index, TStep step)
        where TStep : IFlowStep
    {
        if (step is null)
            throw new ArgumentNullException(nameof(step));
        if (index < 0 || index > _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        _steps.Insert(index, step);
        return CreateConfigurableBuilder(step);
    }

    public virtual IConfigurableStepBuilder ReplaceStep<TStep>(int index, TStep step)
        where TStep : IFlowStep
    {
        if (step is null)
            throw new ArgumentNullException(nameof(step));
        if (index < 0 || index >= _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        _steps[index] = step;
        return CreateConfigurableBuilder(step);
    }
    
    public virtual IConfigurableStepBuilder RemoveStepAt(int index)
    {
        if (index < 0 || index >= _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        var step = _steps[index];
        _steps.RemoveAt(index);
        return CreateConfigurableBuilder(step);
    }
    


    protected abstract IConfigurableStepBuilder CreateConfigurableBuilder<TStep>(TStep step)
        where TStep : IFlowStep;
}