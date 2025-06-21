using FFlow.Core;

namespace FFlow.Observability.Listeners;

/// <summary>
/// A simple implementation of <see cref="IFlowEventListener"/> that logs workflow and step events to the console.
/// </summary>
public class ConsoleFlowEventListener : IFlowEventListener
{
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        Console.WriteLine($"[{Timestamp()}] [Workflow Started] {BuildWorkflowName(workflow)}");
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        Console.WriteLine($"[{Timestamp()}] [Workflow Completed] {BuildWorkflowName(workflow)}");
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        Console.WriteLine($"[{Timestamp()}] [Workflow Failed] {BuildWorkflowName(workflow)} - Exception: {exception.Message}");
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        Console.WriteLine($"[{Timestamp()}] [Step Started] {step.GetType().Name} - Context ID: {context.Id}");
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        Console.WriteLine($"[{Timestamp()}] [Step Completed] {step.GetType().Name} - Context ID: {context.Id}");
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        Console.WriteLine($"[{Timestamp()}] [Step Failed] {step.GetType().Name} - Context ID: {context.Id} - Exception: {exception.Message}");
    }

    private static string BuildWorkflowName(IWorkflow workflow)
    {
        if (workflow.MetadataStore.TryGet("name", out string? name))
            return name;
        
        
        return "Unnamed Workflow";
    }
    
    private static string Timestamp()
    {
        return DateTime.UtcNow.ToString("o");
    }
}