namespace FFlow.Core;

public abstract class WorkflowBuilderBase
{
    private readonly List<IFlowStep> _steps = new();
    private IStepTemplateRegistry? _stepTemplateRegistry;
    private IFlowContext? _flowContext;
    private Type? _contextType;

    public virtual IReadOnlyList<IFlowStep> Steps => _steps.AsReadOnly();

    public abstract IWorkflow Build();
    
    public virtual IStepTemplateRegistry? StepTemplateRegistry
    {
        get => _stepTemplateRegistry;
        set
        {
            if (_stepTemplateRegistry is not null && value is not null && _stepTemplateRegistry != value)
                throw new InvalidOperationException("StepTemplateRegistry has already been set and cannot be changed.");
            _stepTemplateRegistry = value;
        }
    }
    
    public virtual IFlowContext? FlowContext
    {
        get => _flowContext;
        set
        {
            if (_flowContext is not null && value is not null && _flowContext != value)
                throw new InvalidOperationException("FlowContext has already been set and cannot be changed.");
            _flowContext = value;
        }
    }
    
    public virtual Type? ContextType
    {
        get => _contextType;
        set
        {
            if (_contextType is not null && value is not null && _contextType != value)
                throw new InvalidOperationException("ContextType has already been set and cannot be changed.");
            _contextType = value;
        }
    }

    public virtual void AddStep(IFlowStep step)
    {
        ArgumentNullException.ThrowIfNull(step);

        if (StepTemplateRegistry is not null && StepTemplateRegistry.TryGetOverridenDefaults(step.GetType(), out var configure))
            configure(step);

        _steps.Add(step);
    }

    public virtual void InsertStepAt(int index, IFlowStep step)
    {
        ArgumentNullException.ThrowIfNull(step);
        if (index < 0 || index > _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        if (StepTemplateRegistry is not null && StepTemplateRegistry.TryGetOverridenDefaults(step.GetType(), out var configure))
            configure(step);
        
        _steps.Insert(index, step);
    }

    public virtual void ReplaceStep(int index, IFlowStep step)
    {
        ArgumentNullException.ThrowIfNull(step);
        if (index < 0 || index >= _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        
        if (StepTemplateRegistry is not null && StepTemplateRegistry.TryGetOverridenDefaults(step.GetType(), out var configure))
            configure(step);
        
        _steps[index] = step;
    }
    
    public virtual void RemoveStepAt(int index)
    {
        if (index < 0 || index >= _steps.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the steps list.");

        var step = _steps[index];
        _steps.RemoveAt(index);
    }
    
    public abstract void SetErrorHandlingStep(IFlowStep step);
}