using FFlow.Core;

namespace FFlow.Extensions;

public static class IWorkflowMetadataStoreExtensions
{
    public static IWorkflowMetadataStore SetName(this IWorkflowMetadataStore store, string name)
    {
        store.Set("name", name);
        return store;
    }
    
    public static string? GetName(this IWorkflowMetadataStore store)
    {
        return store.Get<string>("name");
    }
    
    public static bool TryGetName(this IWorkflowMetadataStore store, out string? name)
    {
        return store.TryGet("name", out name);
    }
    
    public static bool HasName(this IWorkflowMetadataStore store)
    {
        return store.HasKey("name");
    }
    
    public static IWorkflowMetadataStore SetDescription(this IWorkflowMetadataStore store, string description)
    {
        store.Set("description", description);
        return store;
    }
    
    public static string? GetDescription(this IWorkflowMetadataStore store)
    {
        return store.Get<string>("description");
    }
    
    public static bool TryGetDescription(this IWorkflowMetadataStore store, out string? description)
    {
        return store.TryGet("description", out description);
    }
    
    public static bool HasDescription(this IWorkflowMetadataStore store)
    {
        return store.HasKey("description");
    }
}