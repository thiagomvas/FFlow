using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Observability.Listeners;

/// <summary>
/// Records a timeline of workflow and step events with relative timestamps for observability and debugging purposes.
/// </summary>
public class TimelineRecorder : IFlowEventListener
{
    private readonly List<string> _timeline = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        _stopwatch.Restart();
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Workflow Started] {BuildWorkflowName(workflow)}");
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        _stopwatch.Stop();
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Workflow Completed] {BuildWorkflowName(workflow)} - Duration: {_stopwatch.ElapsedMilliseconds} ms");
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        _stopwatch.Stop();
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Workflow Failed] {BuildWorkflowName(workflow)} - Exception: {exception.Message} - Duration: {_stopwatch.ElapsedMilliseconds} ms");
        
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Step Started] {step.GetType().Name} - Context ID: {context.Id}");
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Step Completed] {step.GetType().Name} - Context ID: {context.Id}");
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        _timeline.Add($"[{_stopwatch.Elapsed:g}] [Step Failed] {step.GetType().Name} - Context ID: {context.Id} - Exception: {exception.Message}");
    }
    
    public IReadOnlyList<string> GetTimeline() => _timeline.AsReadOnly();
    private static string BuildWorkflowName(IWorkflow workflow)
    {
        if (workflow.MetadataStore.TryGet("name", out string? name))
            return name;
        
        
        return "Unnamed Workflow";
    }
}