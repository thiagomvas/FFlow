namespace FFlow.Core;

/// <summary>
/// Represents a store for workflow metadata, allowing storage and retrieval of key-value pairs.
/// </summary>
public interface IWorkflowMetadataStore
{
    /// <summary>
    /// Sets a value in the metadata store for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">The value to store.</param>
    void Set<T>(string key, T value);

    /// <summary>
    /// Retrieves a value from the metadata store for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <returns>The value associated with the key, or <c>null</c> if the key does not exist.</returns>
    T? Get<T>(string key);

    /// <summary>
    /// Attempts to retrieve a value from the metadata store for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">When this method returns, contains the value associated with the key, 
    /// or <c>null</c> if the key does not exist.</param>
    /// <returns><c>true</c> if the key exists and the value was retrieved; otherwise, <c>false</c>.</returns>
    bool TryGet<T>(string key, out T? value);

    /// <summary>
    /// Checks if the metadata store contains the specified key.
    /// </summary>
    /// <param name="key">The key to check for existence.</param>
    /// <returns><c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
    bool HasKey(string key);
}