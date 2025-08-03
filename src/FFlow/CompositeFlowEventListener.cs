using FFlow.Core;

namespace FFlow;

/// <summary>
/// Represents a composite event listener that aggregates multiple <see cref="IFlowEventListener"/> instances.
/// </summary>
public class CompositeFlowEventListener : IFlowEventListener
{
    private readonly List<IFlowEventListener> _listeners;
    public IReadOnlyList<IFlowEventListener> Listeners => _listeners;
    
    public CompositeFlowEventListener(IEnumerable<IFlowEventListener> listeners)
    {
        ArgumentNullException.ThrowIfNull(listeners);

        _listeners = new List<IFlowEventListener>(listeners);
    }
    
    public void AddListener(IFlowEventListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);

        _listeners.Add(listener);
    }
    
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        foreach (var listener in _listeners)
        {
            listener.OnWorkflowStarted(workflow);
        }
        
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        foreach (var listener in _listeners)
        {
            listener.OnWorkflowCompleted(workflow);
        }
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(workflow);
        ArgumentNullException.ThrowIfNull(exception);

        foreach (var listener in _listeners)
        {
            listener.OnWorkflowFailed(workflow, exception);
        }
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(context);

        foreach (var listener in _listeners)
        {
            listener.OnStepStarted(step, context);
        }
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(context);

        foreach (var listener in _listeners)
        {
            listener.OnStepCompleted(step, context);
        }
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exception);

        foreach (var listener in _listeners)
        {
            listener.OnStepFailed(step, context, exception);
        }
    }
}