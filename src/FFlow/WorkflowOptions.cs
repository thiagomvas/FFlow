using FFlow.Core;

namespace FFlow;

public class WorkflowOptions
{
    public TimeSpan? StepTimeout { get; set; } = null;
    public TimeSpan? GlobalTimeout { get; set; } = null;
    
    
    public Func<IFlowStep, IFlowStep>? StepDecoratorFactory { get; set; } = null;

    public WorkflowOptions AddStepDecorator(Func<IFlowStep, IFlowStep> decorator)
    {
        if (StepDecoratorFactory is null)
        {
            StepDecoratorFactory = decorator;
            return this;
        }

        var currentDecorator = StepDecoratorFactory;
        StepDecoratorFactory = step => decorator(currentDecorator(step));
        return this;
    }
}