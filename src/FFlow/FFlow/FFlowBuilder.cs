using FFlow.Core;

namespace FFlow;

public class FFlowBuilder<TInput> : IWorkflowBuilder<TInput>
{
    private readonly List<IFlowStep> _steps = new List<IFlowStep>();
    public IWorkflowBuilder<TInput> StartWith<TStep>() where TStep : IFlowStep
    {
        var step = Activator.CreateInstance<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder<TInput> StartWith(Func<IFlowContext, Task> setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder<TInput> Then<TStep>() where TStep : IFlowStep
    {
        var step = Activator.CreateInstance<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder<TInput> Then(Func<IFlowContext, Task> setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder<TInput> Finally<TStep>() where TStep : IFlowStep
    {
        var step = Activator.CreateInstance<TStep>();
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder<TInput> Finally(Func<IFlowContext, Task> setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return this;
    }

    public IWorkflow<TInput> Build()
    {
        var context = new InMemoryFFLowContext();
        return new Workflow<TInput>(_steps, context);
    }
}