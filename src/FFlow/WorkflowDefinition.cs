using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;

namespace FFlow;

/// <summary>
/// Base class for defining workflow templates.
/// </summary>
public abstract class WorkflowDefinition : IWorkflowDefinition
{
    private readonly IServiceProvider? _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowDefinition"/> class.
    /// </summary>
    /// <param name="serviceProvider">Optional service provider for resolving dependencies.</param>
    protected WorkflowDefinition(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Configures the workflow steps using the provided builder.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    public abstract void OnConfigure(WorkflowBuilderBase builder);

    /// <summary>
    /// Returns a delegate that configures workflow execution options.
    /// </summary>
    /// <returns>An action that modifies <see cref="WorkflowOptions"/>.</returns>
    public abstract Action<WorkflowOptions> OnConfigureOptions();

    /// <summary>
    /// Sets the metadata store used by the workflow.
    /// </summary>
    /// <param name="metadataStore">The metadata store instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="metadataStore"/> is null.</exception>
    public void SetMetadataStore(IWorkflowMetadataStore metadataStore)
    {
        MetadataStore = metadataStore ?? throw new ArgumentNullException(nameof(metadataStore));
    }

    /// <summary>
    /// Builds the workflow using the configured steps and options.
    /// </summary>
    /// <returns>The constructed workflow instance.</returns>
    public IWorkflow Build()
    {
        var builder = CreateBuilder();
        var result = builder.Build();
        if (result is Workflow flow)
        {
            flow.MetadataStore = MetadataStore;
        }

        return result;
    }

    /// <summary>
    /// Creates a new workflow builder instance with the configured options.
    /// </summary>
    /// <returns>Creates a configured builder based on this definition.</returns>
    public WorkflowBuilderBase CreateBuilder()
    {
        var builder = new nFFlowBuilder(_serviceProvider);
        OnConfigure(builder);
        builder.WithOptions(OnConfigureOptions());
        return builder;
    }
    
    /// <summary>
    /// Describes the workflow as a graph structure.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public WorkflowGraph Describe(VisualizationFlags flags = VisualizationFlags.IgnoreInputOutputSetters)
    {
        var builder = CreateBuilder();
        return builder.Describe(flags);
    }

    /// <summary>
    /// Gets the metadata store associated with the workflow.
    /// </summary>
    public IWorkflowMetadataStore MetadataStore { get; private set; } = new InMemoryMetadataStore();
}
