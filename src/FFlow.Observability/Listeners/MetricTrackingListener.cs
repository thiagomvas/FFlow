using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Metrics;

namespace FFlow.Observability.Listeners;

/// <summary>
/// An implementation of <see cref="IFlowEventListener"/> that tracks metrics for workflow and step events
/// using the provided metrics sink.
/// </summary>
/// <typeparam name="TSink">
/// The type of the metrics sink used for tracking. Must implement <see cref="IMetricsSink"/>.
/// </typeparam>
public class MetricTrackingListener<TSink> : IFlowEventListener where TSink : class, IMetricsSink
{
    private readonly TSink _metricsSink;

    private const string StepStartKeyPrefix = "__metrics.step_start:";
    private const string WorkflowStartKey = "__metrics.workflow_start";

    public MetricTrackingListener(TSink metricsSink)
    {
        _metricsSink = metricsSink ?? throw new ArgumentNullException(nameof(metricsSink));
    }

    public void OnWorkflowStarted(IWorkflow workflow)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));

        workflow.GetContext().SetSingleValue(_metricsSink);
        
        workflow.MetadataStore.Set(WorkflowStartKey, DateTime.UtcNow);

        _metricsSink.Increment("workflow.started", new Dictionary<string, string>
        {
            { "workflow.name", workflow.MetadataStore.Get<string>("name") ?? "Unnamed Workflow" }
        });
    }

    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));

        if (workflow.MetadataStore.TryGet<DateTime>(WorkflowStartKey, out var start))
        {
            var duration = DateTime.UtcNow - start;
            _metricsSink.RecordTiming("workflow.duration", duration, new Dictionary<string, string>
            {
                { "workflow.name", workflow.MetadataStore.Get<string>("name") ?? "Unnamed Workflow" }
            });
        }

        _metricsSink.Increment("workflow.completed", new Dictionary<string, string>
        {
            { "workflow.name", workflow.MetadataStore.Get<string>("name") ?? "Unnamed Workflow" }
        });
    }

    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        if (workflow is null) throw new ArgumentNullException(nameof(workflow));
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        if (workflow.MetadataStore.TryGet<DateTime>(WorkflowStartKey, out var start))
        {
            var duration = DateTime.UtcNow - start;
            _metricsSink.RecordTiming("workflow.duration", duration, new Dictionary<string, string>
            {
                { "workflow.name", workflow.MetadataStore.Get<string>("name") ?? "Unnamed Workflow" },
                { "status", "failed" }
            });
        }

        _metricsSink.Increment("workflow.failed", new Dictionary<string, string>
        {
            { "workflow.name", workflow.MetadataStore.Get<string>("name") ?? "Unnamed Workflow" },
            { "error.message", exception.Message }
        });
    }

    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        if (step is null) throw new ArgumentNullException(nameof(step));
        if (context is null) throw new ArgumentNullException(nameof(context));

        context.SetValue(StepStartKey(step), DateTime.UtcNow);

        _metricsSink.Increment("step.started", new Dictionary<string, string>
        {
            { "step.name", step.GetType().Name },
        });
    }

    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        RecordStepTiming(step, context, "completed");

        _metricsSink.Increment("step.completed", new Dictionary<string, string>
        {
            { "step.name", step.GetType().Name },
        });
    }

    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        RecordStepTiming(step, context, "failed");

        _metricsSink.Increment("step.failed", new Dictionary<string, string>
        {
            { "step.name", step.GetType().Name },
            { "error.message", exception.Message }
        });
    }

    private void RecordStepTiming(IFlowStep step, IFlowContext context, string status)
    {
        if (context.GetValue<DateTime>(StepStartKey(step)) is DateTime start)
        {
            var duration = DateTime.UtcNow - start;

            _metricsSink.RecordTiming("step.duration", duration, new Dictionary<string, string>
            {
                { "step.name", step.GetType().Name },
                { "status", status }
            });
        }
    }

    private static string StepStartKey(IFlowStep step)
        => $"{StepStartKeyPrefix}{step.GetType().FullName}";
}
