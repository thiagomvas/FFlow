namespace FFlow.Core;

public interface IWorkflowMetadataStore
{
    void Set<T>(string key, T value);
    T? Get<T>(string key);
    bool TryGet<T>(string key, out T? value);
    bool HasKey(string key);
}