using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using FFlow.Steps.DotNet;

namespace FFlow.Samples.BasicServer.Workflows;

public class BuildAndTestWorkflow : WorkflowDefinition
{
    private readonly MetricTrackingListener<InMemoryMetricsSink> _metricTrackingListener;
    private readonly ILogger<BuildAndTestWorkflow> _logger;
    
    public BuildAndTestWorkflow(MetricTrackingListener<InMemoryMetricsSink> metricTrackingListener, ILogger<BuildAndTestWorkflow> logger)
    {
        _logger = logger;
        _metricTrackingListener = metricTrackingListener;
        MetadataStore.SetName("Build and Test FFlow")
            .SetDescription("This workflow clones the FFlow repository, builds it, and runs tests on it.");
    }
    public override void OnConfigure(IWorkflowBuilder builder)
    {
        var clonePath = Path.Combine(Path.GetTempPath(), "FFlow");
        builder.StartWith((ctx, _) =>
            {
                if (Directory.Exists(clonePath))
                {
                    Directory.Delete(clonePath, true);
                    Directory.CreateDirectory(clonePath);
                }
            })
            .RunCommand($"git clone https://github.com/thiagomvas/FFlow.git --depth 1 --branch master {clonePath}",
                step => step.OutputHandler = str => _logger.LogDebug(str))
            .DotnetBuild(clonePath)
            .DotnetTest(clonePath, step => step.NoBuild = true)
            .If(ctx => !ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>().Success, 
                (_, _) => throw new Exception("Tests failed."))
            .Finally((_, _) =>
            {
                if (Directory.Exists(clonePath))
                {
                    Directory.Delete(clonePath, true);
                }
            });

    }

    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
            options.WithEventListener(_metricTrackingListener);
        };
    }
}