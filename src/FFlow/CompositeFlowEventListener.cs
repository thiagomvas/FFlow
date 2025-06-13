using FFlow.Core;

namespace FFlow;

public class CompositeFlowEventListener : IFlowEventListener
{
    private readonly List<IFlowEventListener> _listeners;
    
    public CompositeFlowEventListener(IEnumerable<IFlowEventListener> listeners)
    {
        if (listeners is null) throw new ArgumentNullException(nameof(listeners));
        
        _listeners = new List<IFlowEventListener>(listeners);
    }
    
    public void AddListener(IFlowEventListener listener)
    {
        if (listener is null) throw new ArgumentNullException(nameof(listener));
        
        _listeners.Add(listener);
    }
    
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));
        
        foreach (var listener in _listeners)
        {
            listener.OnWorkflowStarted(workflow);
        }
        
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));
        
        foreach (var listener in _listeners)
        {
            listener.OnWorkflowCompleted(workflow);
        }
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));
        if (exception is null) throw new ArgumentNullException(nameof(exception));
        
        foreach (var listener in _listeners)
        {
            listener.OnWorkflowFailed(workflow, exception);
        }
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (context is null) throw new ArgumentNullException(nameof(context));
        
        foreach (var listener in _listeners)
        {
            listener.OnStepStarted(step, context);
        }
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (context is null) throw new ArgumentNullException(nameof(context));
        
        foreach (var listener in _listeners)
        {
            listener.OnStepCompleted(step, context);
        }
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (exception is null) throw new ArgumentNullException(nameof(exception));
        
        foreach (var listener in _listeners)
        {
            listener.OnStepFailed(step, context, exception);
        }
    }
}