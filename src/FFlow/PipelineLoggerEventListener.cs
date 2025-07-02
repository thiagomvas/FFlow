using FFlow.Core;

namespace FFlow;

public class PipelineLoggerEventListener : IFlowEventListener
{
    private DateTimeOffset _startTime;
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        _startTime = DateTimeOffset.UtcNow;
        Console.WriteLine($"Workflow started at {_startTime}");
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        var endTime = DateTimeOffset.UtcNow;
        var duration = endTime - _startTime;
        Console.WriteLine($"Workflow completed at {endTime}.\nElapsed {FormattedTime(duration)}.");
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        Console.WriteLine($"Workflow failed with exception: {exception.Message}");
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        LogStepConclusion(step, context);
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        Console.WriteLine($"Step {GetStepName(step)} failed with exception: {exception.Message}");
    }

    private void LogStepConclusion(IFlowStep step, IFlowContext context)
    {
        if (step.GetType().GetCustomAttributes(typeof(SilentStepAttribute), false).Length != 0)
        {
            // Skip logging for silent steps
            return;
        }
        var stepName = GetStepName(step);
        var now = DateTime.UtcNow;
        var output = context.GetOutputFor(step.GetType());

        var msg = $"[{now}] {stepName} completed successfully.";
        if (output != null)
        {
            msg += $" Output: {output}";
        }
        Console.WriteLine(msg);
    }

    private string GetStepName(IFlowStep step)
    {
        var attribute = step.GetType().GetCustomAttributes(typeof(StepNameAttribute), false)
            .FirstOrDefault() as StepNameAttribute;

        if (attribute != null)
            return attribute.Name;

        return step.GetType().Name;
    }
    
    private string GetFormattedStepTags(IFlowStep step)
    {
        var tags = step.GetType().GetCustomAttributes(typeof(StepTagsAttribute), false)
            .Cast<StepTagsAttribute>()
            .SelectMany(attr => attr.Tags)
            .ToArray();

        return string.Join(", ", tags);
    }

    private string FormattedTime(TimeSpan timeSpan)
    {
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
    }
}

