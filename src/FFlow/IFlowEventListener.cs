using FFlow.Core;

namespace FFlow;

public interface IFlowEventListener
{
    /// <summary>
    /// Invoked when a workflow starts.
    /// </summary>
    /// <param name="workflow">The workflow that started.</param>
    void OnWorkflowStarted(IWorkflow workflow);

    /// <summary>
    /// Invoked when a workflow completes successfully.
    /// </summary>
    /// <param name="workflow">The workflow that completed.</param>
    void OnWorkflowCompleted(IWorkflow workflow);

    /// <summary>
    /// Invoked when a workflow fails with an exception.
    /// </summary>
    /// <param name="workflow">The workflow that failed.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    void OnWorkflowFailed(IWorkflow workflow, Exception exception);
    
    
    /// <summary>
    /// Invoked when a step starts.
    /// </summary>
    /// <param name="step">The step that started.</param>
    /// <param name="context">The current flow context.</param>
    void OnStepStarted(IFlowStep step, IFlowContext context);

    /// <summary>
    /// Invoked when a step completes successfully.
    /// </summary>
    /// <param name="step">The step that completed.</param>
    /// <param name="context">The current flow context.</param>
    void OnStepCompleted(IFlowStep step, IFlowContext context);

    /// <summary>
    /// Invoked when a step fails with an exception.
    /// </summary>
    /// <param name="step">The step that failed.</param>
    /// <param name="context">The current flow context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception);
}