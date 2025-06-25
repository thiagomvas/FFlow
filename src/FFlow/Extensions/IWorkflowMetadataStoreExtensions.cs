using FFlow.Core;

namespace FFlow.Extensions;

/// <summary>
/// Extension methods for <see cref="IWorkflowMetadataStore"/> to manage common metadata fields.
/// </summary>
public static class IWorkflowMetadataStoreExtensions
{
    /// <summary>
    /// Sets the "name" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <param name="name">The name to set.</param>
    /// <returns>The updated metadata store.</returns>
    public static IWorkflowMetadataStore SetName(this IWorkflowMetadataStore store, string name)
    {
        store.Set("name", name);
        return store;
    }

    /// <summary>
    /// Gets the "name" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <returns>The name, or null if not set.</returns>
    public static string? GetName(this IWorkflowMetadataStore store)
    {
        return store.Get<string>("name");
    }

    /// <summary>
    /// Tries to get the "name" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <param name="name">When this method returns, contains the name if present; otherwise, null.</param>
    /// <returns><c>true</c> if the name is present; otherwise, <c>false</c>.</returns>
    public static bool TryGetName(this IWorkflowMetadataStore store, out string? name)
    {
        return store.TryGet("name", out name);
    }

    /// <summary>
    /// Determines whether the "name" metadata key exists.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <returns><c>true</c> if the name key exists; otherwise, <c>false</c>.</returns>
    public static bool HasName(this IWorkflowMetadataStore store)
    {
        return store.HasKey("name");
    }

    /// <summary>
    /// Sets the "description" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <param name="description">The description to set.</param>
    /// <returns>The updated metadata store.</returns>
    public static IWorkflowMetadataStore SetDescription(this IWorkflowMetadataStore store, string description)
    {
        store.Set("description", description);
        return store;
    }

    /// <summary>
    /// Gets the "description" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <returns>The description, or null if not set.</returns>
    public static string? GetDescription(this IWorkflowMetadataStore store)
    {
        return store.Get<string>("description");
    }

    /// <summary>
    /// Tries to get the "description" metadata value.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <param name="description">When this method returns, contains the description if present; otherwise, null.</param>
    /// <returns><c>true</c> if the description is present; otherwise, <c>false</c>.</returns>
    public static bool TryGetDescription(this IWorkflowMetadataStore store, out string? description)
    {
        return store.TryGet("description", out description);
    }

    /// <summary>
    /// Determines whether the "description" metadata key exists.
    /// </summary>
    /// <param name="store">The workflow metadata store.</param>
    /// <returns><c>true</c> if the description key exists; otherwise, <c>false</c>.</returns>
    public static bool HasDescription(this IWorkflowMetadataStore store)
    {
        return store.HasKey("description");
    }
}
