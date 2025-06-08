namespace FFlow.Core;

public interface IFlowContext
{
    TInput GetInput<TInput>();
    void SetInput<TInput>(TInput input);
    T Get<T>(string key);
    void Set<T>(string key, T value);
    bool TryGet<T>(string key, out T value);
    
}