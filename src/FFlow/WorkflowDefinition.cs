using FFlow.Core;

namespace FFlow;

public abstract class WorkflowDefinition : IWorkflowDefinition
{
    private readonly IServiceProvider? _serviceProvider;
    
    protected WorkflowDefinition(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }
    
    public abstract void OnConfigure(IWorkflowBuilder builder);
    public abstract Action<WorkflowOptions> OnConfigureOptions();
    public void SetMetadataStore(IWorkflowMetadataStore metadataStore)
    {
        MetadataStore = metadataStore ?? throw new ArgumentNullException(nameof(metadataStore));
    }
    public IWorkflow Build()
    {
        var builder = new FFlowBuilder(_serviceProvider);
        OnConfigure(builder);
        builder.WithOptions(OnConfigureOptions());
        var result = builder.Build();
        if (result is Workflow flow)
        {
            flow.MetadataStore = MetadataStore;
        }
        
        return result;
    }

    public IWorkflowMetadataStore MetadataStore { get; private set; } = new InMemoryMetadataStore();
}