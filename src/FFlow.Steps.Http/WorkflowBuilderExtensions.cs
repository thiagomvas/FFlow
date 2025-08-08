using FFlow.Core;

namespace FFlow.Steps.Http;

public static class WorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a custom configured HTTP request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase SendHttpRequest(this WorkflowBuilderBase builder,
        Action<HttpRequestStep> configure)
    {
        var step = new HttpRequestStep();
        configure(step);

        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds an HTTP GET request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpGet(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Get;
            configure(step);
        });
    }
    
    /// <summary>
    /// Adds an HTTP GET request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP GET request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpGet(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpGet(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP POST request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPost(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Post;
            configure(step);
        });
    }
    
    /// <summary>
    /// Adds an HTTP POST request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP POST request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPost(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpPost(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP PUT request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPut(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Put;
            configure(step);
        });
    }
    
    /// <summary>
    /// Adds an HTTP PUT request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP PUT request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPut(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpPut(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP DELETE request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpDelete(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Delete;
            configure(step);
        });
    }
    
    /// <summary>
    /// Adds an HTTP DELETE request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP DELETE request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpDelete(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpDelete(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP PATCH request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPatch(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Patch;
            configure(step);
        });
    }
    
    /// <summary>
    /// Adds an HTTP PATCH request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP PATCH request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpPatch(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpPatch(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP HEAD request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpHead(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Head;
            configure(step);
        });
    }
    
    
    /// <summary>
    /// Adds an HTTP HEAD request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP HEAD request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpHead(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpHead(step => step.Url = url);
    }

    /// <summary>
    /// Adds an HTTP OPTIONS request step to the workflow.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the HttpRequestStep.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpOptions(this WorkflowBuilderBase builder, Action<HttpRequestStep> configure)
    {
        return builder.SendHttpRequest(step =>
        {
            step.Method = HttpMethod.Options;
            configure(step);
        });
    }
    
    
    /// <summary>
    /// Adds an HTTP OPTIONS request step to the workflow with a specified URL.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="url">The URL to send the HTTP OPTIONS request to.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase HttpOptions(this WorkflowBuilderBase builder, string url)
    {
        return builder.HttpOptions(step => step.Url = url);
    }
}