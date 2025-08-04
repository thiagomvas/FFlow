using FFlow.Core;

namespace FFlow.Steps.Http;

public static class WorkflowBuilderExtensions
{
    public static WorkflowBuilderBase SendHttpRequest(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        var step = new HttpRequestStep();
        configure(step);

        builder.AddStep(step);
        return builder;
    }

    public static WorkflowBuilderBase HttpGet(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Get;
            configure(step);
        });
    }
    
}