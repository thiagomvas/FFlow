namespace FFlow.Core;

public class nForwardingWorkflowBuilder : WorkflowBuilderBase
{
    private readonly WorkflowBuilderBase _inner;

    public nForwardingWorkflowBuilder(WorkflowBuilderBase inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public override IStepTemplateRegistry? StepTemplateRegistry
    {
        get => _inner.StepTemplateRegistry;
        set => _inner.StepTemplateRegistry = value;
    }
    
    public override IFlowContext? FlowContext
    {
        get => _inner.FlowContext;
        set => _inner.FlowContext = value;
    }
    
    public override Type? ContextType
    {
        get => _inner.ContextType;
        set => _inner.ContextType = value;
    }
    
    public override IWorkflow Build() => _inner.Build();

    public override void AddStep(IFlowStep step) => _inner.AddStep(step);

    public override void InsertStepAt(int index, IFlowStep step) => _inner.InsertStepAt(index, step);

    public override void ReplaceStep(int index, IFlowStep step) => _inner.ReplaceStep(index, step);

    public override void RemoveStepAt(int index) => _inner.RemoveStepAt(index);

    public override IReadOnlyList<IFlowStep> Steps => _inner.Steps;
}