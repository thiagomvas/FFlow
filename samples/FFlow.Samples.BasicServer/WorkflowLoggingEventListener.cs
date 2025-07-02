using FFlow.Core;
using FFlow.Extensions;

namespace FFlow.Samples.BasicServer;

public class WorkflowLoggingEventListener : IFlowEventListener
{
    private readonly ILogger<WorkflowLoggingEventListener> _logger;
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        _logger.LogInformation("Workflow started: {WorkflowName}", workflow.MetadataStore.GetName());
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        _logger.LogInformation("Workflow completed: {WorkflowName}", workflow.MetadataStore.GetName());
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        _logger.LogError(exception, "Workflow failed: {WorkflowName}", workflow.MetadataStore.GetName());
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        _logger.LogInformation("Step started: {StepName}",
            step.GetType().Name);
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        _logger.LogInformation("Step completed: {StepName}",
            step.GetType().Name);
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        _logger.LogError(exception, "Step failed: {StepName}", step.GetType().Name);
    }
}