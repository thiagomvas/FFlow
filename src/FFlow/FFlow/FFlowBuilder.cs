using FFlow.Core;

namespace FFlow;

public class FFlowBuilder<TInput> : IWorkflowBuilder<TInput>
{
    private readonly List<IFlowStep> _steps = new List<IFlowStep>();
    private IFlowStep? _errorHandler;
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

    public IWorkflowBuilder<TInput> OnAnyError<TStep>() where TStep : IFlowStep
    {
        var step = Activator.CreateInstance<TStep>();
        _errorHandler = step ?? throw new InvalidOperationException($"Could not create instance of {typeof(TStep).Name}");
        return this;
    }

    public IWorkflowBuilder<TInput> OnAnyError(Func<IFlowContext, Task> errorHandlerAction)
    {
        if (errorHandlerAction == null) throw new ArgumentNullException(nameof(errorHandlerAction));
        
        _errorHandler = new DelegateFlowStep(errorHandlerAction);
        return this;
    }

    public IWorkflow<TInput> Build()
    {
        var context = new InMemoryFFLowContext();
        var result = new Workflow<TInput>(_steps, context);
        
        if (_errorHandler != null)
        {
            result.SetGlobalErrorHandler(_errorHandler);
        }
        
        return result;
    }
}