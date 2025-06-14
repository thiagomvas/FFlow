using FFlow;
using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestFlowEventListener : IFlowEventListener
{
    public int WorkflowStartedCount { get; private set; }
    public int WorkflowCompletedCount { get; private set; }
    public int WorkflowFailedCount { get; private set; }
    public int StepStartedCount { get; private set; }
    public int StepCompletedCount { get; private set; }
    public int StepFailedCount { get; private set; }
    
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        WorkflowStartedCount++;
    }
    
    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        WorkflowCompletedCount++;
    }
    
    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        WorkflowFailedCount++;
    }
    
    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        StepStartedCount++;
    }
    
    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        StepCompletedCount++;
    }
    
    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        StepFailedCount++;
    }
}