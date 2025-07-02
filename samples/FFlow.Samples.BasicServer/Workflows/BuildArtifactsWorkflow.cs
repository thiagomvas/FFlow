using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using FFlow.Steps.DotNet;
using FFlow.Steps.Shell;

namespace FFlow.Samples.BasicServer.Workflows;

public class BuildArtifactsWorkflow : WorkflowDefinition
{
    private readonly MetricTrackingListener<InMemoryMetricsSink> _metricTrackingListener;
    private readonly ILogger<BuildArtifactsWorkflow> _logger;

    public BuildArtifactsWorkflow(MetricTrackingListener<InMemoryMetricsSink> metricTrackingListener, ILogger<BuildArtifactsWorkflow> logger)
    {
        _logger = logger;
        _metricTrackingListener = metricTrackingListener;
        MetadataStore.SetName("Build Artifacts for FFlow")
            .SetDescription("This workflow builds artifacts for the application and returns the file path to it.");
    }

    public override void OnConfigure(IWorkflowBuilder builder)
    {
        var clonePath = Path.Combine(Path.GetTempPath(), "fflow");
        var outPath = Path.Combine(Path.GetTempPath(), "fflow-artifacts");
        builder.StartWith((ctx, _) =>
            {
                if (Directory.Exists(clonePath))
                {
                    Directory.Delete(clonePath, true);
                }
                if (Directory.Exists(outPath))
                {
                    Directory.Delete(outPath, true);
                }
                Directory.CreateDirectory(clonePath);
                Directory.CreateDirectory(outPath);
            })
            .RunCommand($"git clone https://github.com/thiagomvas/FFlow.git --depth 1 --branch master {clonePath}",
                step => step.OutputHandler = str => _logger.LogDebug(str))
            .DotnetBuild(clonePath)
            .DotnetTest(clonePath, step => step.NoBuild = true)
            .DotnetPublish(clonePath, step =>
            {
                step.Configuration = "Release";
                step.Output = outPath;
            })
            .RunCommand($"tar -a -c -f {Path.Combine(outPath, "fflow.zip")} -C {outPath} .",
                step => step.OutputHandler = str => _logger.LogDebug(str))
            .Then((ctx, _) =>
            {
                ctx.SetValue("artifactsPath", Path.Combine(outPath, "fflow.zip"));
            })
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